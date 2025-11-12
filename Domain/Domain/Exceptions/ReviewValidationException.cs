using PodpiskaNaSemena.Domain.Exceptions;

namespace PodpiskaNaSemena.Domain.Exceptions
{
    public class ReviewValidationException : DomainException
    {
        public ReviewValidationException(string message) : base(message) { }

        // Дополнительные конструкторы
        public ReviewValidationException(int reviewId, string message)
            : base($"Отзыв {reviewId}: {message}") { }

        public ReviewValidationException(int userId, int seedId, string message)
            : base($"Пользователь {userId}, семена {seedId}: {message}") { }
    }
}