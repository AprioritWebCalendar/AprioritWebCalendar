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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().Property(p => p.Id).UseSqlServerIdentityColumn();
            builder.Entity<ApplicationUser>().HasMany(u => u.UserRoles).WithOne().HasForeignKey(u => u.UserId).IsRequired();
            builder.Entity<ApplicationRole>().HasMany(r => r.UserRoles).WithOne().HasForeignKey(r => r.RoleId).IsRequired();

        }
    }
}
