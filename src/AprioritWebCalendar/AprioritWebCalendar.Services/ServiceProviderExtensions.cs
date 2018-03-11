using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AprioritWebCalendar.Data;
using AprioritWebCalendar.Data.Entities;
using AprioritWebCalendar.Services.Identity;
using AprioritWebCalendar.Data.Interfaces;

namespace AprioritWebCalendar.Services
{
    /// <summary>
    /// The methods of this class will be called from Startup
    /// to inject services from DAL (Data Access Level),
    /// that can't be related with "Web".
    /// </summary>
    public static class ServiceProviderExtensions
    {
        public static void UseAppDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(connectionString);
            });
        }

        public static void UseIdentity(this IServiceCollection services)
        {
            services.AddTransient<IUserValidator<User>, CustomUserValidator>();

            services.AddIdentity<User, Role>(opts =>
            {
                opts.Password.RequiredLength = 6;
                opts.Password.RequiredUniqueChars = 4;

                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireDigit = true;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;

                opts.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppDbContext>()
               .AddDefaultTokenProviders();
        }

        public static void UseRepository(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}
