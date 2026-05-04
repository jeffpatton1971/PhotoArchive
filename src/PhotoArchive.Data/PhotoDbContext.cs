using Microsoft.EntityFrameworkCore;
using PhotoArchive.Core.Entities;

namespace PhotoArchive.Data
{

    public class PhotoDbContext : DbContext
    {
        public PhotoDbContext(DbContextOptions<PhotoDbContext> options)
            : base(options)
        {
        }

        public DbSet<Photo> Photos => Set<Photo>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Photo>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasIndex(x => x.Slug).IsUnique();
                entity.HasIndex(x => new { x.Year, x.Month, x.Day });
                entity.HasIndex(x => new { x.Month, x.Day });

                entity.Property(x => x.Source).IsRequired();
                entity.Property(x => x.OriginalUrl).IsRequired();
            });
        }
    }
}