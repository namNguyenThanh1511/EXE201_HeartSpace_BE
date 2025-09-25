using HeartSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeartSpace.Infrastructure.Configuration
{
    public class ClientProfileConfiguration : IEntityTypeConfiguration<ClientProfile>
    {
        public void Configure(EntityTypeBuilder<ClientProfile> builder)
        {
            builder.ToTable("ClientProfiles");
            builder.HasKey(cp => cp.Id);
            builder.Property(cp => cp.Id)
                        .ValueGeneratedOnAdd()
                        .IsRequired();
            builder.Property(cp => cp.Bio)
                .IsRequired()
                .HasMaxLength(2000);
            builder.Property(cp => cp.Preferences)
                .HasMaxLength(1000);
            builder.Property(cp => cp.Goals)
                .HasMaxLength(1000);
            builder.Property(cp => cp.MedicalHistory)
                .HasMaxLength(2000);
            builder.Property(cp => cp.MentalHealthStatus)
                .HasMaxLength(2000);
            builder.Property(cp => cp.CreatedAt)
                .IsRequired();
            builder.Property(cp => cp.UpdatedAt)
                .IsRequired();
            // Relationships
            builder.HasOne(cp => cp.Client)
                .WithOne(u => u.ClientProfile)
                .HasForeignKey<ClientProfile>(cp => cp.ClientId)
                .OnDelete(DeleteBehavior.Cascade);// xóa profile khi xóa user
        }
    }
}
