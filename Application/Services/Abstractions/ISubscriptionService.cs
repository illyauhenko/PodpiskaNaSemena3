using PodpiskaNaSemena.Application.Models.Subscription;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PodpiskaNaSemena.Application.Services.Abstractions
{
    /// <summary>
    /// Сервис для управления подписками на семена
    /// Отвечает за жизненный цикл подписок: создание, отмена, продление
    /// </summary>
    public interface ISubscriptionService
    {
        /// <summary>
        /// Создает новую подписку на семена
        /// Вычисляет даты начала и окончания на основе типа подписки
        /// </summary>
        /// <param name="request">Данные для создания подписки</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>Созданная подписка</returns>
        Task<SubscriptionResponse> CreateSubscriptionAsync(CreateSubscriptionRequest request, CancellationToken ct = default);

        /// <summary>
        /// Отменяет активную подписку
        /// Проверяет возможность отмены согласно бизнес-правилам
        /// </summary>
        /// <param name="subscriptionId">Идентификатор подписки для отмены</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        Task CancelSubscriptionAsync(int subscriptionId, CancellationToken ct = default);

        /// <summary>
        /// Получает детальную информацию о подписке
        /// Включает информацию об оплате и оставшемся времени
        /// </summary>
        /// <param name="id">Идентификатор подписки</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>Детальные данные подписки</returns>
        Task<SubscriptionDetailsResponse> GetSubscriptionAsync(int id, CancellationToken ct = default);

        /// <summary>
        /// Получает все подписки указанного пользователя
        /// Используется для отображения списка подписок в личном кабинете
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>Список подписок пользователя</returns>
        Task<IReadOnlyList<SubscriptionResponse>> GetUserSubscriptionsAsync(int userId, CancellationToken ct = default);

        /// <summary>
        /// Получает все активные подписки в системе
        /// Используется для администрирования и отчетности
        /// </summary>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>Список активных подписок</returns>
        Task<IReadOnlyList<SubscriptionResponse>> GetActiveSubscriptionsAsync(CancellationToken ct = default);

        /// <summary>
        /// Проверяет есть ли у пользователя активная подписка на указанные семена
        /// Используется для предотвращения дублирования подписок
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="seedId">Идентификатор семян</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>True если активная подписка существует</returns>
        Task<bool> HasActiveSubscriptionAsync(int userId, int seedId, CancellationToken ct = default);
    }
}