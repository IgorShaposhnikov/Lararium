using Lararium.Video.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lararium.Persistence.Configurations.Video
{
    internal sealed class VideoEntityConfiguration : IEntityTypeConfiguration<VideoEntity>
    {
        public void Configure(EntityTypeBuilder<VideoEntity> builder)
        {
            builder.ToTable("videos");

            builder.HasKey(x => x.Id);

            // Mapped inherited base properties from MediaEntity
            builder.Property(x => x.FileSize)
                .IsRequired();

            builder.Property(x => x.FileType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.FileExt)
                .IsRequired()
                .HasMaxLength(20);

            // VideoEntity specific properties
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Summary)
                .HasMaxLength(1000);

            // One-to-Many associations mapping to shadow keys in current migrations
            builder.HasMany(x => x.Tags)
                .WithOne()
                .HasForeignKey("VideoEntityId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Actors)
                .WithOne()
                .HasForeignKey("VideoEntityId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
