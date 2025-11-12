using PodpiskaNaSemena.Domain.Entities;
using PodpiskaNaSemena.Domain.Enums;

namespace PodpiskaNaSemena.Domain.Repositories.Abstractions
{
    /// <summary>
    /// Репозиторий для работы с подписками на семена
    /// Содержит методы для управления жизненным циклом подписок
    /// </summary>
    public interface ISubscriptionRepository : IRepository<Subscription, int>
    {
        /// <summary>
        /// Получает активные подписки для пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Коллекция активных подписок</returns>
        Task<IReadOnlyList<Subscription>> GetActiveSubscriptionsForUserAsync(
            int userId,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Проверяет есть ли у пользователя активная подписка на указанные семена
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="seedId">Идентификатор семян</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>True если активная подписка существует</returns>
        Task<bool> HasActiveSubscriptionAsync(
            int userId,
            int seedId,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Получает подписки, срок действия которых истекает в ближайшие дни
        /// </summary>
        /// <param name="daysThreshold">Количество дней до истечения</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Коллекция истекающих подписок</returns>
        Task<IReadOnlyList<Subscription>> GetExpiringSubscriptionsAsync(
            int daysThreshold,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Получает подписки по статусу (активные, отмененные, завершенные)
        /// </summary>
        /// <param name="status">Статус подписки</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Коллекция подписок с указанным статусом</returns>
        Task<IReadOnlyList<Subscription>> GetSubscriptionsByStatusAsync(
            SubscriptionStatus status,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Получает все подписки на указанные семена
        /// </summary>
        /// <param name="seedId">Идентификатор семян</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Коллекция подписок</returns>
        Task<IReadOnlyList<Subscription>> GetSubscriptionsBySeedAsync(
            int seedId,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Получает количество активных подписок на указанные семена
        /// </summary>
        /// <param name="seedId">Идентификатор семян</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Количество активных подписок</returns>
        Task<int> GetActiveSubscriptionsCountAsync(
            int seedId,
            CancellationToken cancellationToken = default
        );
    }
}