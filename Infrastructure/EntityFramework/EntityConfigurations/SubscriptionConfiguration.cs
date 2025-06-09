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

            builder.Property(s => s.UserId).IsRequired();
            builder.Property(s => s.SeedId).IsRequired();

            builder.Property(s => s.StartDate)
                .IsRequired();

            builder.Property(s => s.EndDate)
                .IsRequired();

            builder.Property(s => s.Status)
                .IsRequired()
                .HasConversion(
                    s => s.ToString(),
                    s => (SubscriptionStatus)Enum.Parse(typeof(SubscriptionStatus), s));

            builder.HasOne(s => s.Payment)
                .WithOne()
                .HasForeignKey<Payment>(p => p.SubscriptionId);
        }
    }
}