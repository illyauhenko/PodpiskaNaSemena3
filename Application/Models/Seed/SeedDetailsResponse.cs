namespace PodpiskaNaSemena.Application.Models.Seed
{
    /// <summary>
    /// Подробная информация о семенах
    /// </summary>
    public sealed record SeedDetailsResponse(
        int Id,
        string Name,
        string Description,
        decimal Price,
        bool IsAvailable,
        double AverageRating,
        int ReviewCount,     // Количество отзывов
        int SubscriptionCount // Количество активных подписок
    );
}