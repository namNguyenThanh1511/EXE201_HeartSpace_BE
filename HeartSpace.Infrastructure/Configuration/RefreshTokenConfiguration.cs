﻿using HeartSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeartSpace.Infrastructure.Configuration
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");
            builder.HasKey(rt => rt.Id);
            builder.Property(rt => rt.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();
            builder.Property(rt => rt.Token)
                .IsRequired()
                .HasMaxLength(500);
            builder.Property(rt => rt.ExpiryDate)
                .IsRequired();
            // Relationships
            builder.HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            // Indexes
            builder.HasIndex(rt => rt.Token)
                .IsUnique();
            builder.HasIndex(rt => rt.UserId);
            builder.HasIndex(rt => rt.ExpiryDate);
        }
    }
}
