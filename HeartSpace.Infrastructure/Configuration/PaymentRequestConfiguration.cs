using HeartSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeartSpace.Infrastructure.Configuration
{
    public class PaymentRequestConfiguration : IEntityTypeConfiguration<PaymentRequest>
    {
        public void Configure(EntityTypeBuilder<PaymentRequest> builder)
        {
            builder.ToTable("PaymentRequests");
            builder.Property(p => p.Id)
                           .ValueGeneratedOnAdd()
                           .IsRequired();
            builder.Property(p => p.AppointmentId)
                .IsRequired();
            builder.Property(p => p.ConsultantId)
                .IsRequired();
            builder.Property(pr => pr.RequestAmount)
                .HasColumnType("decimal(18,2)");
            builder.Property(pr => pr.Status).HasConversion<string>();
            builder.Property(pr => pr.BankAccount)
                .HasMaxLength(100);
            builder.Property(pr => pr.BankName)
                .HasMaxLength(255);
            builder.Property(pr => pr.CreatedAt);
            builder.Property(pr => pr.ProcessedAt);
            // Thiết lập quan hệ với Appointment
            builder
                .HasOne(pr => pr.Appointment)
                .WithOne(a => a.PaymentRequest)
                .HasForeignKey<PaymentRequest>(pr => pr.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);
            // Thiết lập quan hệ với User (Consultant)
            builder
                .HasOne(pr => pr.Consultant)
                .WithMany(u => u.PaymentRequests)
                .HasForeignKey(pr => pr.ConsultantId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
