using HeartSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeartSpace.Infrastructure.Configuration
{
    public class ConsultingConfiguration : IEntityTypeConfiguration<Consulting>
    {
        public void Configure(EntityTypeBuilder<Consulting> builder)
        {
            builder.ToTable("Consultings");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(255);
            // Quan hệ 1-nhiều với bảng trung gian ConsultantConsulting
            builder
                .HasMany(c => c.ConsultantConsultings)
                .WithOne(cc => cc.Consulting)
                .HasForeignKey(cc => cc.ConsultingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
