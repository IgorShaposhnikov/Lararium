using Lararium.Core;
using Lararium.Media.Module;
using Lararium.Persistence.Extensions;
using Lararium.Video.Models;
using Microsoft.EntityFrameworkCore;

namespace Lararium.Persistence
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<LarariumUser> Users { get; init; }
        public DbSet<VideoEntity> Videos { get; init; }
        public DbSet<MediaTag> MediaTags { get; init; }
        public DbSet<Actor> Actors { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.UseSnakeCaseNamingConvention();

            builder.Entity<LarariumUser>().ToTable("users");
            builder.Entity<VideoEntity>().ToTable("videos");
            builder.Entity<MediaTag>().ToTable("media_tags");
            builder.Entity<Actor>().ToTable("media_actors");
        }
    }
}
