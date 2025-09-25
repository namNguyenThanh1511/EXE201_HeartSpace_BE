using HeartSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace HeartSpace.Infrastructure.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();
            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.PhoneNumber)
                .HasMaxLength(15);
            builder.HasIndex(u => u.PhoneNumber).IsUnique();
            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);
            builder.HasIndex(u => u.Username).IsUnique();
            builder.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(255);
            builder.Property(u => u.DateOfBirth);
            builder.Property(u => u.Avatar)
                .HasMaxLength(255);
            builder.Property(u => u.Identifier)
                .HasMaxLength(50);
            //builder.HasIndex(u => u.Identifier).IsUnique();
            builder.Property(u => u.UserRole)
                .HasConversion<string>()
                .IsRequired();
            builder.Property(u => u.Gender);
            builder.Property(u => u.IsActive)
                .IsRequired();
            builder.Property(u => u.CreatedAt)
                .IsRequired();
            builder.Property(u => u.UpdatedAt);
            // Quan hệ 1-n: User (Client) - Appointments
            builder.HasMany(u => u.ClientAppointments)
                .WithOne(a => a.Client)
                .HasForeignKey(a => a.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
            // Quan hệ 1-n: User (Consultant) - Appointments
            builder.HasMany(u => u.ConsultantAppointments)
                .WithOne(a => a.Consultant)
                .HasForeignKey(a => a.ConsultantId)
                .OnDelete(DeleteBehavior.Restrict);
            // Quan hệ 1-n: User (Consultant) - Schedules
            builder.HasMany(u => u.ConsultantSchedules)
                .WithOne(s => s.Consultant)
                .HasForeignKey(s => s.ConsultantId)
                .OnDelete(DeleteBehavior.Cascade);
            // Quan hệ 1-1: User - ClientProfile
            builder.HasOne(u => u.ClientProfile)
                .WithOne(cp => cp.Client)
                .HasForeignKey<ClientProfile>(cp => cp.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
            // Quan hệ 1-1: User - ConsultantProfile
            builder.HasOne(u => u.ConsultantProfile)
                .WithOne(cp => cp.Consultant)
                .HasForeignKey<ConsultantProfile>(cp => cp.ConsultantId)
                .OnDelete(DeleteBehavior.Cascade);
            //builder.Property(u => u.IsEmailConfirmed)
            //    .IsRequired();
            //builder.Property(u => u.EmailConfirmationToken)
            //    .HasMaxLength(255);
            //builder.Property(u => u.PasswordResetToken)
            //    .HasMaxLength(255);
            //builder.Property(u => u.PasswordResetTokenExpiryTime);

        }
    }
}
