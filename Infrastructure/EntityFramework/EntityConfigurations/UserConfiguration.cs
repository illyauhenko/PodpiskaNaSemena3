using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PodpiskaNaSemena.Domain.Entities;
using PodpiskaNaSemena.Domain.ValueObjects;

namespace PodpiskaNaSemena.EntityFramework.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Username)
                .HasConversion(
                    u => u.Value,
                    u => new Username(u))
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.Email)
                .HasConversion(
                    e => e.Value,
                    e => new Email(e))
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.CreatedAt)
                .IsRequired();

            builder.HasMany(u => u.Subscriptions)
                .WithOne()
                .HasForeignKey(s => s.UserId);

            builder.HasMany(u => u.Reviews)
                .WithOne()
                .HasForeignKey(r => r.UserId);
        }
    }
}