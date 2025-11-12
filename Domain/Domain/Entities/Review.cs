using PodpiskaNaSemena.Domain.Entities.Base;
using PodpiskaNaSemena.Domain.Exceptions;
using PodpiskaNaSemena.Domain.ValueObjects;

namespace PodpiskaNaSemena.Domain.Entities
{
    public class Review : Entity<int>
    {
        public int UserId { get; }
        public int SeedId { get; }
        public Rating Rating { get; private set; }
        public Comment? Comment { get; private set; }
        public DateTime CreatedAt { get; }
        public DateTime? UpdatedAt { get; private set; }
        public bool IsEdited => UpdatedAt.HasValue;

        public Review(int userId, int seedId, Rating rating, Comment? comment)
        {
            ValidateReview(userId, seedId, rating);

            UserId = userId;
            SeedId = seedId;
            Rating = rating;
            Comment = comment;
            CreatedAt = DateTime.UtcNow;
        }

        // Метод для обновления отзыва
        public void UpdateReview(Rating newRating, Comment? newComment, User editor)
        {
            if (editor == null)
                throw new DomainException("Пользователь не может быть null");

            if (!CanEdit(editor))
                throw new ReviewAccessDeniedException(editor.Id, Id);

            ValidateReview(UserId, SeedId, newRating);

            Rating = newRating;
            Comment = newComment;
            UpdatedAt = DateTime.UtcNow;
        }

        // Метод для удаления отзыва
        public void DeleteReview(User deleter)
        {
            if (deleter == null)
                throw new DomainException("Пользователь не может быть null");

            if (!CanDelete(deleter))
                throw new ReviewAccessDeniedException(deleter.Id, Id);

            // В реальности здесь была бы пометка на удаление
        }

        // Проверки прав
        public bool CanEdit(User user)
            => user != null && (user.Id == UserId || user.IsAdmin);

        public bool CanDelete(User user)
            => user != null && (user.Id == UserId || user.IsAdmin);

        // Вспомогательные методы
        public string GetCommentText()
        {
            var commentText = Comment?.Value ?? string.Empty;
            return IsEdited ? $"{commentText} (отредактировано)" : commentText;
        }

        public string GetRatingStars() => Rating.ToStarsString();

        public bool IsPositiveReview() => Rating.IsGood || Rating.IsExcellent;

        // Валидация отзыва
        private static void ValidateReview(int userId, int seedId, Rating rating)
        {
            if (userId <= 0)
                throw new ReviewValidationException("Некорректный идентификатор пользователя");

            if (seedId <= 0)
                throw new ReviewValidationException("Некорректный идентификатор семян");

            if (rating == null)
                throw new ReviewValidationException("Рейтинг обязателен");
        }
    }
}