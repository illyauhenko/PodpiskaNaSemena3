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

            builder.Property(u => u.IsAdmin)
                .IsRequired()
                .HasDefaultValue(false); // По умолчанию не админ

         
            builder.HasMany(u => u.Subscriptions)
                .WithMany(s => s.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserSubscriptions",
                    j => j
                        .HasOne<Subscription>()
                        .WithMany()
                        .HasForeignKey("SubscriptionId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<User>()
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("UserId", "SubscriptionId");
                        j.HasIndex("SubscriptionId");
                        j.ToTable("UserSubscriptions");
                    });

            // Связь с отзывами (один-ко-многим)
            builder.HasMany(u => u.Reviews)
                .WithOne()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Индексы для производительности
            builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("IX_Users_Email");

            builder.HasIndex(u => u.Username)
                .HasDatabaseName("IX_Users_Username");

            builder.HasIndex(u => u.IsAdmin)
                .HasDatabaseName("IX_Users_IsAdmin")
                .HasFilter("\"IsAdmin\" = true"); // Только для админов
        }
    }
}