

namespace PodpiskaNaSemena.Domain.Enums
{
    public enum PaymentStatus
    {
        /// <summary> Платеж инициализирован, но не обработан </summary>
        Pending = 0,

        /// <summary> Платеж успешно завершен </summary>
        Completed = 1,

        /// <summary> Ошибка при обработке платежа </summary>
        Failed = 2,

        /// <summary> Платеж возвращен пользователю </summary>
        Refunded = 3,

        /// <summary> Платеж отменен до завершения </summary>
        Canceled = 4
    }
}
