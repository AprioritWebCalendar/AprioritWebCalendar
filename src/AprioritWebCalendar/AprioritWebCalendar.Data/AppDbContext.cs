using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AprioritWebCalendar.Data.Models;

namespace AprioritWebCalendar.Data
{
    public partial class AppDbContext : 
        IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected AppDbContext()
        {
        }

        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<UserCalendar> UserCalendars { get; set; }
        public DbSet<EventCalendar> EventCalendars { get; set; }
        public DbSet<Period> EventPeriods { get; set; }
        public DbSet<Invitation> Invitations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().Property(p => p.Id).UseSqlServerIdentityColumn();
            builder.Entity<ApplicationUser>().HasMany(u => u.UserRoles).WithOne().HasForeignKey(u => u.UserId).IsRequired();
            builder.Entity<ApplicationRole>().HasMany(r => r.UserRoles).WithOne().HasForeignKey(r => r.RoleId).IsRequired();

            builder.Entity<UserCalendar>().HasKey(k => new { k.UserId, k.CalendarId });
            builder.Entity<EventCalendar>().HasKey(k => new { k.EventId, k.CalendarId });
            builder.Entity<Invitation>().HasKey(k => new { k.EventId, k.UserId, k.InvitatorId });
            EFCore.UseDateDiff(builder);
        }
    }
}
