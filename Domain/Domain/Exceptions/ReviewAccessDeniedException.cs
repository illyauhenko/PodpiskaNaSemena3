using PodpiskaNaSemena.Domain.Exceptions;

namespace PodpiskaNaSemena.Domain.Exceptions
{
    public class ReviewAccessDeniedException : DomainException
    {
        public ReviewAccessDeniedException(int userId, int reviewId)
            : base($"Пользователь {userId} не имеет доступа к отзыву {reviewId}") { }

        // Дополнительные конструкторы для удобства
        public ReviewAccessDeniedException(int userId, int reviewId, string operation)
            : base($"Пользователь {userId} не может выполнить операцию '{operation}' с отзывом {reviewId}") { }

        public ReviewAccessDeniedException(string userName, int reviewId)
            : base($"Пользователь '{userName}' не имеет доступа к отзыву {reviewId}") { }
    }
}