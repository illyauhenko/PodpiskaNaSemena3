using PodpiskaNaSemena.Application.Models.Base;

namespace PodpiskaNaSemena.Application.Models.Payment
{
    /// <summary>
    /// Ответ с данными о платеже
    /// Возвращается после создания или получения платежа
    /// </summary>
    public sealed record PaymentResponse(
        int Id,                 // Идентификатор платежа
        int SubscriptionId,     // Связанная подписка
        decimal Amount,         // Сумма платежа
        string PaymentMethod,   // Способ оплаты
        DateTime PaymentDate,   // Когда был создан платеж
        string Status           // "Pending", "Completed", "Failed"
    ) : ResponseModel<int>(Id); 
}