namespace PodpiskaNaSemena.Application.Models.Subscription
{
    /// <summary>
    /// Подробная информация о подписке
    /// Используется для страницы деталей подписки
    /// </summary>
    public sealed record SubscriptionDetailsResponse(
        int Id,
        string Username,
        string SeedName,
        DateTime StartDate,
        DateTime EndDate,
        string Status,
        bool IsPaid,
        bool IsActive,
        decimal Price,          // Стоимость подписки
        string? PaymentStatus,  // "Pending", "Completed", "Failed"
        int DaysRemaining,      // Осталось дней действия
        bool CanCancel,         // Можно ли отменить подписку
        bool CanRenew           // Можно ли продлить подписку
    );
}