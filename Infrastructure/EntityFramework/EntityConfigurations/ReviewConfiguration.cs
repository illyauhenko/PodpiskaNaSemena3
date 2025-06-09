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

            builder.Property(r => r.UserId).IsRequired();
            builder.Property(r => r.SeedId).IsRequired();

            builder.Property(r => r.Rating)
                .HasConversion(
                    r => r.Value,
                    r => new Rating(r))
                .IsRequired();

            builder.Property(r => r.Comment)
                .HasConversion(
                    c => c!.Value,
                    c => new Comment(c))
                .HasMaxLength(1000);

            builder.Property(r => r.CreatedAt)
                .IsRequired();
        }
    }
}