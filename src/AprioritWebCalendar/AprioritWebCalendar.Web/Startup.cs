using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using AprioritWebCalendar.Services;

namespace AprioritWebCalendar.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Enabling DI configs using IOptions<T>.
            services.AddOptions();

            services.UseAppDbContext(Configuration.GetConnectionString("DefaultConnection"));
            services.UseIdentity();
            services.UseRepository();

            services.AddAutoMapper();

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
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Routing if we have an SPA.

            //app.Use(async (context, next) =>
            //{
            //    await next();

            //    if (context.Response.StatusCode == 404 &&
            //        !Path.HasExtension(context.Request.Path.Value) &&
            //        !context.Request.Path.Value.StartsWith("/api/"))
            //    {
            //        context.Request.Path = "/index.html";
            //        await next();
            //    }
            //});

            app.UseMvcWithDefaultRoute();
            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}
