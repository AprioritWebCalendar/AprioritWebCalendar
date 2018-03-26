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

namespace AprioritWebCalendar.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;

            var custConfigBuilder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath + "\\configs\\")
                .AddJsonFile("jwtOptions.json", true, true);

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

            services.UseAppDbContext(Configuration.GetConnectionString("DefaultConnection"));
            services.UseIdentity();
            services.UseRepository();

            services.MapServices();

            services.AddAutoMapper();

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
                });

            services.AddAuthorization(opts =>
            {
                opts.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });

            #endregion

            services
                .AddMvc()
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
                }); ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseMvc();
        }
    }
}
