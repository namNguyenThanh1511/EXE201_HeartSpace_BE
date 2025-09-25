using HeartSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeartSpace.Infrastructure.Configuration
{
    public class ConsultantProfileConfiguration : IEntityTypeConfiguration<ConsultantProfile>
    {
        public void Configure(EntityTypeBuilder<ConsultantProfile> builder)
        {
            builder.ToTable("ConsultantProfiles");
            builder.Property(cp => cp.Id)
                        .ValueGeneratedOnAdd()
                        .IsRequired();
            builder.HasKey(cp => cp.Id);
            builder.Property(cp => cp.Bio)
                .IsRequired()
                .HasMaxLength(2000);
            builder.Property(cp => cp.Specialization)
                .IsRequired()
                .HasMaxLength(255);
            builder.Property(cp => cp.ExperienceYears)
                .IsRequired();
            builder.Property(cp => cp.HourlyRate)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            builder.Property(cp => cp.Certifications)
                .HasMaxLength(1000);
            builder.Property(cp => cp.CreatedAt)
                .IsRequired();
            builder.Property(cp => cp.UpdatedAt)
                .IsRequired();
            // Relationships
            builder.HasOne(cp => cp.Consultant)
                .WithOne(u => u.ConsultantProfile)
                .HasForeignKey<ConsultantProfile>(cp => cp.ConsultantId)
                .OnDelete(DeleteBehavior.Cascade);// xóa profile khi xóa user
        }
    }

}
