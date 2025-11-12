using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PodpiskaNaSemena.Domain.Entities;
using PodpiskaNaSemena.Domain.Enums;
using PodpiskaNaSemena.Domain.ValueObjects;

namespace PodpiskaNaSemena.EntityFramework.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.SubscriptionId)
                .IsRequired();

            // маппинг Value Objects
            builder.Property(p => p.Amount)
                .HasConversion(
                    a => a.Value,
                    a => new Amount(a))
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.PaymentMethod)
                .HasConversion(
                    pm => pm.Value,
                    pm => new PaymentMethod(pm))
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.PaymentDate)
                .IsRequired();

            builder.Property(p => p.Status)
                .IsRequired()
                .HasConversion(
                    s => s.ToString(),
                    s => (PaymentStatus)Enum.Parse(typeof(PaymentStatus), s))
                .HasMaxLength(20);

            builder.Property(p => p.FailureReason)
                .HasMaxLength(500); // Причина отказа

            //  связь с Subscription
            builder.HasOne<Subscription>()
                .WithOne(s => s.Payment)
                .HasForeignKey<Payment>(p => p.SubscriptionId)
                .OnDelete(DeleteBehavior.Cascade); // Удалять платеж при удалении подписки

            //  Индексы для производительности
            builder.HasIndex(p => p.SubscriptionId)
                .IsUnique()
                .HasDatabaseName("IX_Payments_SubscriptionId"); // Уникальный для one-to-one

            builder.HasIndex(p => p.Status)
                .HasDatabaseName("IX_Payments_Status");

            builder.HasIndex(p => p.PaymentDate)
                .HasDatabaseName("IX_Payments_PaymentDate");

            builder.HasIndex(p => new { p.Status, p.PaymentDate })
                .HasDatabaseName("IX_Payments_Status_Date")
                .HasFilter("\"Status\" = 'Pending'"); // Только ожидающие платежи
        }
    }
}