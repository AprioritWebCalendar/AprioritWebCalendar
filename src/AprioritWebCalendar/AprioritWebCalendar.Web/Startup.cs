using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using AprioritWebCalendar.Bootstrap;
using AprioritWebCalendar.Infrastructure.Options;
using AprioritWebCalendar.Web.Jobs;
using AprioritWebCalendar.Web.SignalR.Notifications;
using AprioritWebCalendar.Web.SignalR.Invitations;
using AprioritWebCalendar.Web.SignalR.Calendar;
using AprioritWebCalendar.Web.Formatters;
using AprioritWebCalendar.Web.SignalR.Telegram;

namespace AprioritWebCalendar.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;

            var custConfigBuilder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath + "\\configs\\")
                .AddJsonFile("jwtOptions.json", true, true)
                .AddJsonFile("smtpOptions.json", true, true)
                .AddJsonFile("telegramOptions.json", true, true);

            CustomConfiguration = custConfigBuilder.Build();
        }

        public IConfiguration Configuration { get; }
        public IConfiguration CustomConfiguration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Enabling DI configs using IOptions<T>.
            services.AddOptions();

            services.Configure<JwtOptions>(CustomConfiguration.GetSection("JwtOptions"));
            services.Configure<SmtpOptions>(CustomConfiguration.GetSection("SmtpOptions"));
            services.Configure<TelegramOptions>(CustomConfiguration.GetSection("TelegramOptions"));

            services.UseAppDbContext(Configuration.GetConnectionString("DefaultConnection"));
            services.UseIdentity();
            services.UseRepository();

            services.MapServices();

            services.AddAutoMapper();

            services.AddMemoryCache();

            #region JWT Configuring.

            var jwtOptions = CustomConfiguration.GetSection("JwtOptions");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtOptions[nameof(JwtOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtOptions[nameof(JwtOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions[nameof(JwtOptions.Key)])),

                ValidateLifetime = true
            };

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = tokenValidationParameters;

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = ctx =>
                        {
                            if (ctx.HttpContext.Request.Path.Value.StartsWith("/hub/") && ctx.Request.Query.ContainsKey("token"))
                            {
                                ctx.Token = ctx.Request.Query["token"];
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(opts =>
            {
                opts.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });

            #endregion

            services.AddCors(opt =>
            {
                opt.AddPolicy("AllowAllOrigin", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            services
                .AddMvc(opt =>
                {
                    opt.InputFormatters.Insert(0, new RawJsonBodyInputFormatter());
                })
                .AddJsonOptions(opt =>
                {
                    // JSON sertializer options.

                    // Enable ignoring null values.
                    opt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

                    (opt.SerializerSettings.ContractResolver as DefaultContractResolver).NamingStrategy = null;

#if DEBUG
                    // In debug mode formatting is indented.
                    // In release it's compressed.
                    opt.SerializerSettings.Formatting = Formatting.Indented;
#endif
                });

            services.Configure<MvcOptions>(opt =>
            {
                opt.Filters.Add(new CorsAuthorizationFilterFactory("AllowAllOrigin"));
            });

            services
                .AddSignalR()
                .AddJsonProtocol(conf =>
                {
                    conf.PayloadSerializerSettings.NullValueHandling = NullValueHandling.Ignore;

                    (conf.PayloadSerializerSettings.ContractResolver as DefaultContractResolver).NamingStrategy = null;

#if DEBUG
                    // In debug mode formatting is indented.
                    // In release it's compressed.
                    conf.PayloadSerializerSettings.Formatting = Formatting.Indented;
#endif
                });

            services.AddTransient<NotificationJob>();
            services.AddTransient<InvitationsDeletingJob>();

            services.AddTransient<NotificationHubManager>();
            services.AddTransient<InvitationHubManager>();
            services.AddTransient<CalendarHubManager>();
            services.AddTransient<TelegramHubManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider container)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Routing for SPA.

            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == 404 &&
                    !Path.HasExtension(context.Request.Path.Value) &&
                    !context.Request.Path.Value.StartsWith("/api/"))
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });
            
            app.UseMvcWithDefaultRoute();
            app.UseStaticFiles();

            app.UseCors("AllowAllOrigin");
            app.UseMvc();

            app.UseSignalR(c =>
            {
                c.MapHub<NotificationHub>("/hub/notification");
                c.MapHub<InvitationHub>("/hub/invitation");
                c.MapHub<CalendarHub>("/hub/calendar");
                c.MapHub<TelegramHub>("/hub/telegram");
            });

            JobStarter.RegisterJobs(container);
        }
    }
}
