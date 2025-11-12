using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PodpiskaNaSemena.Domain.Entities;
using PodpiskaNaSemena.Domain.Enums;

namespace PodpiskaNaSemena.EntityFramework.Configurations
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.ToTable("Subscriptions");

            builder.HasKey(s => s.Id);

            // связь через join таблицу
            builder.Property(s => s.SeedId)
                .IsRequired();

            builder.Property(s => s.StartDate)
                .IsRequired();

            builder.Property(s => s.EndDate)
                .IsRequired();

            builder.Property(s => s.Status)
                .IsRequired()
                .HasConversion(
                    s => s.ToString(),
                    s => (SubscriptionStatus)Enum.Parse(typeof(SubscriptionStatus), s))
                .HasMaxLength(20); // Ограничение для enum

            //   связь с Payment (один-к-одному)
            builder.HasOne(s => s.Payment)
                .WithOne()
                .HasForeignKey<Payment>(p => p.SubscriptionId)
                .IsRequired(false) // Payment может быть null (еще не оплачено)
                .OnDelete(DeleteBehavior.Cascade);

            //  Связь с Seed (многие-к-одному)
            builder.HasOne<Seed>()
                .WithMany(s => s.Subscriptions)
                .HasForeignKey(s => s.SeedId)
                .OnDelete(DeleteBehavior.Restrict); // Не удалять Seed если есть подписки

            //  Индексы для производительности
            builder.HasIndex(s => s.SeedId)
                .HasDatabaseName("IX_Subscriptions_SeedId");

            builder.HasIndex(s => s.Status)
                .HasDatabaseName("IX_Subscriptions_Status");

            builder.HasIndex(s => s.StartDate)
                .HasDatabaseName("IX_Subscriptions_StartDate");

            builder.HasIndex(s => s.EndDate)
                .HasDatabaseName("IX_Subscriptions_EndDate");

            //  Составной индекс для поиска активных подписок
            builder.HasIndex(s => new { s.Status, s.EndDate })
                .HasDatabaseName("IX_Subscriptions_Status_EndDate")
                .HasFilter("\"Status\" = 'Active'"); // Только активные подписки
        }
    }
}