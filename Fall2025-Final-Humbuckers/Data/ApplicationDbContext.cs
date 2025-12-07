using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Fall2025_Final_Humbuckers.Models;

namespace Fall2025_Final_Humbuckers.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<GlobalTrack> GlobalTracks { get; set; }
        public DbSet<FavoriteTrack> FavoriteTracks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FavoriteTrack>()
                .HasOne(ft => ft.User)
                .WithMany(u => u.FavoriteTracks)
                .HasForeignKey(ft => ft.UserId)
                .OnDelete(DeleteBehavior.Cascade); // this deletes the favorite if user is deleted
        }
    }
}
