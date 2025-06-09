using PodpiskaNaSemena.Domain.Entities.Base;
using PodpiskaNaSemena.Domain.Exceptions;
using PodpiskaNaSemena.Domain.ValueObjects;

namespace PodpiskaNaSemena.Domain.Entities
{
    public class Review : Entity<int>
    {
        public int UserId { get; }
        public int SeedId { get; }
        public Rating Rating { get; }
        public Comment? Comment { get; }
        public DateTime CreatedAt { get; }

        public Review(int userId, int seedId, Rating rating, Comment? comment)
        {
            if (rating.Value < 1 || rating.Value > 5)
                throw new DomainException("Рейтинг должен быть от 1 до 5.");

            UserId = userId;
            SeedId = seedId;
            Rating = rating;
            Comment = comment;
            CreatedAt = DateTime.UtcNow;
        }
    }
}