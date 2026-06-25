using Lararium.Media.Module;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lararium.Persistence.Configurations.Media
{
    internal sealed class MediaTagConfiguration : IEntityTypeConfiguration<MediaTag>
    {
        public void Configure(EntityTypeBuilder<MediaTag> builder)
        {
            builder.ToTable("media_tags");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
