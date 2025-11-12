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

            builder.Property(s => s.IsAvailable)
                .IsRequired()
                .HasDefaultValue(true); // По умолчанию доступны

            // связь с Subscriptions (один-ко-многим)
            builder.HasMany(s => s.Subscriptions)
                .WithOne()
                .HasForeignKey(s => s.SeedId)
                .OnDelete(DeleteBehavior.Restrict); // Не каскадное удаление

            //  связь с Reviews (один-ко-многим)
            builder.HasMany(s => s.Reviews)
                .WithOne()
                .HasForeignKey(r => r.SeedId)
                .OnDelete(DeleteBehavior.Cascade); // Удалять отзывы при удалении семян

            // Индексы для производительности
            builder.HasIndex(s => s.Name)
                .HasDatabaseName("IX_Seeds_Name");

            builder.HasIndex(s => s.Price)
                .HasDatabaseName("IX_Seeds_Price");

            builder.HasIndex(s => s.IsAvailable)
                .HasDatabaseName("IX_Seeds_IsAvailable")
                .HasFilter("\"IsAvailable\" = true"); // Только доступные семена

            // Составной индекс для поиска
            builder.HasIndex(s => new { s.IsAvailable, s.Price })
                .HasDatabaseName("IX_Seeds_Available_Price");
        }
    }
}