using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PodpiskaNaSemena.Domain.Entities;
using PodpiskaNaSemena.Domain.ValueObjects;

namespace PodpiskaNaSemena.EntityFramework.Configurations
{
    public class SeedConfiguration : IEntityTypeConfiguration<Seed>
    {
        public void Configure(EntityTypeBuilder<Seed> builder)
        {
            builder.ToTable("Seeds");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .HasConversion(
                    n => n.Value,
                    n => new SeedName(n))
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Description)
                .HasConversion(
                    d => d.Value,
                    d => new Description(d))
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(s => s.Price)
                .HasConversion(
                    p => p.Value,
                    p => new Price(p))
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasMany(s => s.Subscriptions)
                .WithOne()
                .HasForeignKey(s => s.SeedId);

            builder.HasMany(s => s.Reviews)
                .WithOne()
                .HasForeignKey(r => r.SeedId);
        }
    }
}