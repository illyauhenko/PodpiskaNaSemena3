using PodpiskaNaSemena.Application.Models.Base;

namespace PodpiskaNaSemena.Application.Models.Review
{
    /// <summary>
    /// Запрос на создание отзыва
    /// Используется когда пользователь оставляет отзыв на семена
    /// </summary>
    public sealed record CreateReviewRequest(
        int UserId,     // Кто оставляет отзыв
        int SeedId,     // На какие семена
        int Rating,     // Оценка 1-5
        string? Comment // Комментарий (необязательный)
    ) : CreateRequestModel;
}