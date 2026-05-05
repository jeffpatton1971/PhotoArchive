using Microsoft.EntityFrameworkCore;
using PhotoArchive.Core.Entities;

namespace PhotoArchive.Data
{
    /// <summary>
    /// The Entity Framework Core database context for the PhotoArchive PostgreSQL database.
    /// </summary>
    public class PhotoDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of <see cref="PhotoDbContext"/> with the given options.
        /// </summary>
        /// <param name="options">The options used to configure the context.</param>
        public PhotoDbContext(DbContextOptions<PhotoDbContext> options)
            : base(options)
        {
        }

        /// <summary>Gets the <see cref="DbSet{TEntity}"/> for <see cref="Photo"/> records.</summary>
        public DbSet<Photo> Photos => Set<Photo>();

        /// <inheritdoc/>
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

                entity.HasIndex(x => x.TakenAt);
                entity.HasIndex(x => x.Gallery);
                entity.HasIndex(x => x.PostId);
                entity.HasIndex(x => x.Source);
            });
        }
    }
}