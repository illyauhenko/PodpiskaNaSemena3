namespace PodpiskaNaSemena.Application.Models.User
{
    /// <summary>
    /// Подробная информация о пользователе (для админа)
    /// </summary>
    public sealed record UserDetailsResponse(
        int Id,
        string Username,
        string Email,
        DateTime CreatedAt,
        bool IsAdmin,                   // Права администратора
        int ActiveSubscriptionsCount,   // Количество активных подписок
        int TotalSubscriptionsCount,    // Всего подписок
        int ReviewsCount                // Количество отзывов
    );
}