using HeartSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeartSpace.Infrastructure.Configuration
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.ToTable("Appointments");
            builder.Property(a => a.Id)
                            .ValueGeneratedOnAdd()
                            .IsRequired();
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Status).HasConversion<string>()
                .IsRequired();
            builder.Property(a => a.PaymentStatus).HasConversion<string>()
                .IsRequired();
            builder.Property(a => a.Notes);
            builder.Property(a => a.OrderCode).ValueGeneratedOnAdd();
            builder.Property(a => a.CreatedAt)
                .IsRequired();
            builder.Property(a => a.UpdatedAt)
                .IsRequired();
            // Relationships
            builder.HasOne(a => a.Schedule)
                .WithMany(s => s.Appointments)
                .HasForeignKey(a => a.ScheduleId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deletion if there are related appointments
            builder.HasOne(a => a.Client)
                .WithMany(u => u.ClientAppointments)
                .HasForeignKey(a => a.ClientId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deletion if there are related appointments
            builder.HasOne(a => a.Consultant)
                .WithMany(u => u.ConsultantAppointments)
                .HasForeignKey(a => a.ConsultantId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deletion if there are related appointments
            builder.HasOne(a => a.Session)
                .WithOne(s => s.Appointment)
                .HasForeignKey<Session>(s => s.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade); // Delete session if appointment is deleted
        }
    }
}
