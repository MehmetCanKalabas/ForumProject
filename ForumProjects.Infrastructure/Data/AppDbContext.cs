using ForumProjects.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ForumProjects.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Users ile ApplicationUser arasındaki ilişkiyi tanımla
            modelBuilder.Entity<Account>()
                .HasOne(u => u.ApplicationUser)
                .WithOne(a => a.Accounts)
                .HasForeignKey<Account>(u => u.UserId);
        }
        public DbSet<Account> Accounts { get; set; }
    }
}
