using PodpiskaNaSemena.Application.Models.Base;

namespace PodpiskaNaSemena.Application.Models.Seed
{
    /// <summary>
    /// Ответ с данными о семенах
    /// </summary>
    public sealed record SeedResponse(
        int Id,
        string Name,
        string Description,
        decimal Price,
        bool IsAvailable,    // Доступны ли для подписки
        double AverageRating // Средний рейтинг из отзывов
    ) : ResponseModel<int>(Id);
}