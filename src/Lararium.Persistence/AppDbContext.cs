using Lararium.Core;
using Microsoft.EntityFrameworkCore;

namespace Lararium.Persistence
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<LarariumUser> Users { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.UseSnakeCaseNamingConvention();

            builder.Entity<LarariumUser>().ToTable("users");
        }
    }
}
