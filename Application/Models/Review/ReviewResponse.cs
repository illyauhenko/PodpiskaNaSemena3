using PodpiskaNaSemena.Application.Models.Base;

namespace PodpiskaNaSemena.Application.Models.Review
{
    /// <summary>
    /// Ответ с данными об отзыве
    /// Используется для отображения отзывов в интерфейсе
    /// </summary>
    public sealed record ReviewResponse(
        int Id,             // Идентификатор отзыва
        string Username,    // Имя пользователя (для отображения)
        string SeedName,    // Название семян (для отображения)
        int Rating,         // Оценка 1-5
        string? Comment,    // Текст отзыва
        DateTime CreatedAt, // Дата создания
        bool IsEdited       // Был ли отзыв отредактирован
    ) : ResponseModel<int>(Id);
}