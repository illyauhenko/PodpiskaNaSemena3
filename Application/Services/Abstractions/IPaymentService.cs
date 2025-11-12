using PodpiskaNaSemena.Application.Models.Payment;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PodpiskaNaSemena.Application.Services.Abstractions
{

    /// <summary>
    /// Сервис для управления платежами за подписки
    /// Отвечает за финансовые операции и интеграцию с платежными системами
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Создает новый платеж для подписки
        /// Инициализирует платеж со статусом "Pending"
        /// </summary>
        /// <param name="request">Данные для создания платежа</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>Созданный платеж</returns>
        Task<PaymentResponse> CreatePaymentAsync(CreatePaymentRequest request, CancellationToken ct = default);

        /// <summary>
        /// Обрабатывает платеж (имитация работы с платежным шлюзом)
        /// В реальной системе здесь была бы интеграция с PayPal/Stripe и т.д.
        /// </summary>
        /// <param name="paymentId">Идентификатор платежа для обработки</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>Обработанный платеж</returns>
        Task<PaymentResponse> ProcessPaymentAsync(int paymentId, CancellationToken ct = default);

        /// <summary>
        /// Получает информацию о платеже по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор платежа</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>Данные платежа</returns>
        Task<PaymentResponse> GetPaymentAsync(int id, CancellationToken ct = default);

        /// <summary>
        /// Отмечает платеж как успешно завершенный
        /// Обновляет статус подписки на "Active" если оплата прошла
        /// </summary>
        /// <param name="paymentId">Идентификатор платежа</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        Task MarkPaymentAsPaidAsync(int paymentId, CancellationToken ct = default);

        /// <summary>
        /// Отмечает платеж как неудачный
        /// Записывает причину отказа для дальнейшего анализа
        /// </summary>
        /// <param name="paymentId">Идентификатор платежа</param>
        /// <param name="reason">Причина отказа платежа</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        Task MarkPaymentAsFailedAsync(int paymentId, string reason, CancellationToken ct = default);

        /// <summary>
        /// Получает платеж по идентификатору подписки
        /// Используется для проверки статуса оплаты подписки
        /// </summary>
        /// <param name="subscriptionId">Идентификатор подписки</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>Платеж связанный с подпиской или null</returns>
        Task<PaymentResponse?> GetPaymentBySubscriptionAsync(int subscriptionId, CancellationToken ct = default);

        /// <summary>
        /// Получает все ожидающие платежи в системе
        /// Используется для администрирования и мониторинга платежей
        /// </summary>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>Список ожидающих платежей</returns>
        Task<IReadOnlyList<PaymentResponse>> GetPendingPaymentsAsync(CancellationToken ct = default);
    }
}