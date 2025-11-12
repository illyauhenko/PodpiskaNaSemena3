using PodpiskaNaSemena.Application.Models.Base;

namespace PodpiskaNaSemena.Application.Models.Subscription
{
    /// <summary>
    /// Запрос на создание подписки
    /// Используется когда пользователь оформляет подписку на семена
    /// </summary>
    public sealed record CreateSubscriptionRequest(
        int UserId,             // Кто оформляет подписку
        int SeedId,             // На какие семена
        string SubscriptionType // "Monthly", "Quarterly", "Yearly"
    ) : CreateRequestModel;
}