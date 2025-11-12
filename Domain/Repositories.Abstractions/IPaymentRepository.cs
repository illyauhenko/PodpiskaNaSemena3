using PodpiskaNaSemena.Domain.Entities;
using PodpiskaNaSemena.Domain.Enums;

namespace PodpiskaNaSemena.Domain.Repositories.Abstractions
{
    /// <summary>
    /// Репозиторий для работы с платежами за подписки
    /// Управляет финансовыми операциями системы
    /// </summary>
    public interface IPaymentRepository : IRepository<Payment, int>
    {
        /// <summary>
        /// Получает платеж по идентификатору подписки
        /// </summary>
        /// <param name="subscriptionId">Идентификатор подписки</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Найденный платеж или null</returns>
        Task<Payment?> GetBySubscriptionIdAsync(
            int subscriptionId,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Получает все ожидающие платежи (со статусом Pending)
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Коллекция ожидающих платежей</returns>
        Task<IReadOnlyList<Payment>> GetPendingPaymentsAsync(
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Получает последний платеж для указанной подписки
        /// </summary>
        /// <param name="subscriptionId">Идентификатор подписки</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Последний платеж или null</returns>
        Task<Payment?> GetLatestBySubscriptionIdAsync(
            int subscriptionId,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Получает платежи по статусу (ожидающие, завершенные, отклоненные)
        /// </summary>
        /// <param name="status">Статус платежа</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Коллекция платежей с указанным статусом</returns>
        Task<IReadOnlyList<Payment>> GetPaymentsByStatusAsync(
            PaymentStatus status,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Получает платежи за указанный период времени
        /// </summary>
        /// <param name="startDate">Начальная дата периода</param>
        /// <param name="endDate">Конечная дата периода</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Коллекция платежей за период</returns>
        Task<IReadOnlyList<Payment>> GetPaymentsByDateRangeAsync(
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default
        );
    }
}