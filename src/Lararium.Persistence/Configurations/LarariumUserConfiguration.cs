using Lararium.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lararium.Persistence.Configurations
{
    internal sealed class LarariumUserConfiguration : IEntityTypeConfiguration<LarariumUser>
    {
        public void Configure(EntityTypeBuilder<LarariumUser> builder)
        {
            builder.ToTable("users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Login)
                .IsRequired()
                .HasMaxLength(100);

            // Enforce unique logins in the database
            builder.HasIndex(x => x.Login)
                .IsUnique();

            builder.Property(x => x.FirstName)
                .HasMaxLength(100);

            builder.Property(x => x.MiddleName)
                .HasMaxLength(100);

            builder.Property(x => x.LastName)
                .HasMaxLength(100);

            builder.Property(x => x.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);
        }
    }
}
