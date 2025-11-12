using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PodpiskaNaSemena.Domain.Entities;
using PodpiskaNaSemena.Domain.ValueObjects;

namespace PodpiskaNaSemena.EntityFramework.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Reviews");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.UserId)
                .IsRequired();

            builder.Property(r => r.SeedId)
                .IsRequired();

            // маппинг Value Objects
            builder.Property(r => r.Rating)
                .HasConversion(
                    r => r.Value,
                    r => new Rating(r))
                .IsRequired();

            builder.Property(r => r.Comment)
                .HasConversion(
                    c => c != null ? c.Value : null,
                    c => c != null ? new Comment(c) : null)
                .HasMaxLength(1000)
                .IsRequired(false); // Комментарий не обязателен

            builder.Property(r => r.CreatedAt)
                .IsRequired();

            builder.Property(r => r.UpdatedAt)
                .IsRequired(false); // Может быть null если не редактировался

            //  Связь с User (многие-к-одному)
            builder.HasOne<User>()
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //  Связь с Seed (многие-к-одному)
            builder.HasOne<Seed>()
                .WithMany(s => s.Reviews)
                .HasForeignKey(r => r.SeedId)
                .OnDelete(DeleteBehavior.Cascade);

            //  Индексы для производительности
            builder.HasIndex(r => r.UserId)
                .HasDatabaseName("IX_Reviews_UserId");

            builder.HasIndex(r => r.SeedId)
                .HasDatabaseName("IX_Reviews_SeedId");

            builder.HasIndex(r => r.Rating)
                .HasDatabaseName("IX_Reviews_Rating");

            builder.HasIndex(r => r.CreatedAt)
                .HasDatabaseName("IX_Reviews_CreatedAt");

            //  Уникальный индекс - один пользователь = один отзыв на семена
            builder.HasIndex(r => new { r.UserId, r.SeedId })
                .IsUnique()
                .HasDatabaseName("IX_Reviews_User_Seed_Unique");
        }
    }
}