using HeartSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeartSpace.Infrastructure.Configuration
{
    public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder.ToTable("Schedules");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(s => s.ConsultantId).IsRequired();
            builder.Property(s => s.StartTime).IsRequired();
            builder.Property(s => s.EndTime).IsRequired();
            builder.Property(s => s.IsAvailable).IsRequired().HasDefaultValue(true);
            builder.Property(s => s.CreatedAt);
            builder.Property(s => s.UpdatedAt);
            // Relationship with User (Consultant)
            builder.HasOne(s => s.Consultant)
                   .WithMany(u => u.ConsultantSchedules) // Assuming Consultant  have a collection of Schedules
                   .HasForeignKey(s => s.ConsultantId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
