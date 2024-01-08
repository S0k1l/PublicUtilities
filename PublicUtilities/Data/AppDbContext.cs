using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PublicUtilities.Models;

namespace PublicUtilities.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<News> News { get; set; }
        public DbSet<Streets> Streets { get; set; }
        public DbSet<PlacesOfResidence> PlacesOfResidence { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<Utilities> Utilities { get; set; }
        public DbSet<Indicators> Indicators { get; set; }
        public DbSet<Departments> Departments { get; set; }
        public DbSet<StatementsTypes> StatementsTypes { get; set; }
        public DbSet<Statements> Statements { get; set; }
        public DbSet<UsersPlacesOfResidence> UsersPlacesOfResidence { get; set; }
        public DbSet<UsersStatements> UsersStatements { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Indicators>()
                .Property(e => e.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<UsersStatements>()
                   .HasKey(pc => new { pc.AppUserId, pc.StatementsId });
            modelBuilder.Entity<UsersStatements>()
                    .HasOne(p => p.Statements)
                    .WithMany(pc => pc.UsersStatements)
                    .HasForeignKey(p => p.StatementsId);
            modelBuilder.Entity<UsersStatements>()
                    .HasOne(p => p.AppUser)
                    .WithMany(pc => pc.UsersStatementsId)
                    .HasForeignKey(c => c.AppUserId);

            modelBuilder.Entity<UsersPlacesOfResidence>()
       .HasKey(pc => new { pc.AppUserId, pc.PlacesOfResidenceId });
            modelBuilder.Entity<UsersPlacesOfResidence>()
                    .HasOne(p => p.PlacesOfResidence)
                    .WithMany(pc => pc.UsersPlacesOfResidence)
                    .HasForeignKey(p => p.PlacesOfResidenceId);
            modelBuilder.Entity<UsersPlacesOfResidence>()
                    .HasOne(p => p.AppUser)
                    .WithMany(pc => pc.UsersPlacesOfResidenceId)
                    .HasForeignKey(c => c.AppUserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
