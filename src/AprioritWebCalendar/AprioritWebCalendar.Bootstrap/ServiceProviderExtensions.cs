using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using AprioritWebCalendar.Data;
using AprioritWebCalendar.Data.Interfaces;
using AprioritWebCalendar.Data.Models;
using AprioritWebCalendar.Business.Identity;
using AprioritWebCalendar.Business.Interfaces;
using AprioritWebCalendar.Business.Services;
using AprioritWebCalendar.Business.Validation;
using AprioritWebCalendar.Business.Telegram;
using AprioritWebCalendar.Business.Recaptcha;

namespace AprioritWebCalendar.Bootstrap
{
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
            services.AddTransient<IUserValidator<ApplicationUser>, CustomUserValidator>();

            services.AddIdentity<ApplicationUser, ApplicationRole>(opts =>
            {
                opts.Password.RequiredLength = 6;
                opts.Password.RequiredUniqueChars = 4;

                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireDigit = true;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;

                opts.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppDbContext>()
               .AddDefaultTokenProviders()
               .AddUserManager<CustomUserManager>();
        }

        public static void UseRepository(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }

        public static void MapServices(this IServiceCollection services)
        {
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IUserAuthenticationService, UserAuthenticationService>();
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<ITelegramVerificationService, TelegramVerificationService>();

            services.AddTransient<ICalendarService, CalendarService>();
            services.AddTransient<ICalendarValidator, CalendarValidator>();

            services.AddTransient<IEventService, EventService>();
            services.AddTransient<IInvitationService, InvitationService>();

            services.AddTransient<INotificationService, NotificationService>();

            services.AddTransient<ICalendarExportService, CalendarExportService>();
            services.AddTransient<ICalendarImportService, CalendarImportService>();

            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<ICacheService, MemoryCacheService>();
            services.AddSingleton<IRandomDataProvider, RandomDataProvider>();
            services.AddSingleton<ITelegramService, TelegramService>();
            services.AddSingleton<ICaptchaService, RecaptchaService>();
        }
    }
}
