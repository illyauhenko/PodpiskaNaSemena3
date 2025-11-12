using System;
using System.Threading;
using System.Threading.Tasks;

namespace PodpiskaNaSemena.Domain.Repositories.Abstractions
{
    /// <summary>
    /// Unit of Work (Единица работы) - паттерн для управления транзакциями
    /// Обеспечивает согласованность данных при работе с несколькими репозиториями
    /// </summary>
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// Репозиторий для работы с пользователями
        /// </summary>
        IUserRepository Users { get; }

        /// <summary>
        /// Репозиторий для работы с семенами
        /// </summary>
        ISeedRepository Seeds { get; }

        /// <summary>
        /// Репозиторий для работы с подписками
        /// </summary>
        ISubscriptionRepository Subscriptions { get; }

        /// <summary>
        /// Репозиторий для работы с платежами
        /// </summary>
        IPaymentRepository Payments { get; }

        /// <summary>
        /// Репозиторий для работы с отзывами
        /// </summary>
        IReviewRepository Reviews { get; }

        // === УПРАВЛЕНИЕ ТРАНЗАКЦИЯМИ ===

        /// <summary>
        /// Сохраняет все изменения в базу данных
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Количество измененных записей</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Начинает новую транзакцию базы данных
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Подтверждает текущую транзакцию
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Откатывает текущую транзакцию
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

        // === СОСТОЯНИЕ СИСТЕМЫ ===

        /// <summary>
        /// Проверяет есть ли активная транзакция
        /// </summary>
        bool HasActiveTransaction { get; }

        /// <summary>
        /// Проверяет был ли объект уже удален
        /// </summary>
        bool IsDisposed { get; }

        // === ДОПОЛНИТЕЛЬНЫЕ МЕТОДЫ ===

        /// <summary>
        /// Проверяет возможность подключения к базе данных
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>True если подключение успешно</returns>
        Task<bool> CanConnectAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Гарантирует что база данных создана (для разработки)
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        Task EnsureCreatedAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Гарантирует что база данных удалена (для тестирования)
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        Task EnsureDeletedAsync(CancellationToken cancellationToken = default);
    }
}