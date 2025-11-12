using PodpiskaNaSemena.Application.Models.Base;

namespace PodpiskaNaSemena.Application.Models.Subscription
{
    /// <summary>
    /// Ответ с данными о подписке
    /// Используется для списка подписок пользователя
    /// </summary>
    public sealed record SubscriptionResponse(
        int Id,
        string Username,        // Имя пользователя (для отображения)
        string SeedName,        // Название семян (для отображения)
        DateTime StartDate,
        DateTime EndDate,
        string Status,          // "Active", "Canceled", "Expired"
        bool IsPaid,            // Оплачена ли подписка
        bool IsActive           // Активна ли на текущий момент
    ) : ResponseModel<int>(Id);
}