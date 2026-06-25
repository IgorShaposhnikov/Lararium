using Lararium.Media.Module;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lararium.Persistence.Configurations.Media
{
    internal sealed class ActorConfiguration : IEntityTypeConfiguration<Actor>
    {
        public void Configure(EntityTypeBuilder<Actor> builder)
        {
            builder.ToTable("media_actors");

            builder.HasKey(x => x.Id);

            // Configure inherited Name property from MediaSubject base class
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.Birthday)
                .IsRequired();
        }
    }
}
