using PodpiskaNaSemena.Application.Models.Base;

namespace PodpiskaNaSemena.Application.Models.Payment
{
    /// <summary>
    /// Запрос на создание платежа для подписки
    /// Используется когда пользователь нажимает "Оплатить"
    /// </summary>
    public sealed record CreatePaymentRequest(
        int SubscriptionId,      // Какую подписку оплачиваем
        decimal Amount,          // Сумма платежа
        string PaymentMethod     // "CreditCard", "PayPal" и т.д.
    ) : CreateRequestModel;      
}