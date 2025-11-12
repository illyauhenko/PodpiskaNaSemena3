namespace PodpiskaNaSemena.Application.Models.Review
{
    /// <summary>
    /// Подробная информация об отзыве
    /// Используется для админки или детального просмотра
    /// </summary>
    public sealed record ReviewDetailsResponse(
        int Id,
        int UserId,         // ID пользователя (для ссылок)
        string Username,
        int SeedId,         // ID семян (для ссылок)
        string SeedName,
        int Rating,
        string? Comment,
        DateTime CreatedAt,
        DateTime? UpdatedAt, // Когда редактировали (если редактировали)
        bool IsEdited
    );
}