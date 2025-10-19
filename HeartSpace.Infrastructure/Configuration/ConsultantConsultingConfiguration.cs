using HeartSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeartSpace.Infrastructure.Configuration
{
    public class ConsultantConsultingConfiguration : IEntityTypeConfiguration<ConsultantConsulting>
    {
        public void Configure(EntityTypeBuilder<ConsultantConsulting> builder)
        {
            builder.ToTable("ConsultantConsultings");
            builder.Property(cc => cc.Id)
                        .ValueGeneratedOnAdd()
                        .IsRequired();
            builder.HasOne(cc => cc.Consulting)
                        .WithMany(c => c.ConsultantConsultings)
                        .HasForeignKey(cc => cc.ConsultingId)
                        .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(cc => cc.Consultant)
                        .WithMany(u => u.ConsultantConsultings)
                        .HasForeignKey(cc => cc.ConsultantId)
                        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
