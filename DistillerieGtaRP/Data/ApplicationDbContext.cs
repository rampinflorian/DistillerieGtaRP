using DistillerieGtaRP.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DistillerieGtaRP.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<Car> Car { get; set; }
        public DbSet<CarLog> CarLogs { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Command> Commands { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Pricing> Pricings { get; set; }
        public DbSet<Parameter> Parameters { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Command>()
                .HasMany(x => x.ApplicationUsers)
                .WithMany(x => x.Commands);
        }
    }
}