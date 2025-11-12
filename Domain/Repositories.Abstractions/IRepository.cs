using PodpiskaNaSemena.Domain.Entities.Base;
using System.Linq.Expressions;

namespace PodpiskaNaSemena.Domain.Repositories.Abstractions

{
    /// <summary>
    /// Базовый интерфейс репозитория для работы с сущностями
    /// Определяет стандартные CRUD операции для всех сущностей системы
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности, должен наследоваться от Entity</typeparam>
    /// <typeparam name="TId">Тип идентификатора сущности</typeparam>
    public interface IRepository<TEntity, TId>
        where TEntity : Entity<TId>
        where TId : struct, IEquatable<TId>
    {
        /// <summary>
        /// Получает сущность по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор сущности</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Найденная сущность или null если не найдена</returns>
        Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получает все сущности данного типа
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Коллекция всех сущностей</returns>
        Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Находит сущности по условию (LINQ выражение)
        /// </summary>
        /// <param name="predicate">Условие фильтрации</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Коллекция найденных сущностей</returns>
        Task<IReadOnlyList<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Добавляет новую сущность в репозиторий
        /// </summary>
        /// <param name="entity">Сущность для добавления</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Обновляет существующую сущность в репозитории
        /// </summary>
        /// <param name="entity">Сущность для обновления</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Удаляет сущность из репозитория
        /// </summary>
        /// <param name="entity">Сущность для удаления</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Проверяет существование сущности с указанным идентификатором
        /// </summary>
        /// <param name="id">Идентификатор для проверки</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>True если сущность существует, иначе False</returns>
        Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получает общее количество сущностей в репозитории
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Количество сущностей</returns>
        Task<int> CountAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Получает количество сущностей, удовлетворяющих условию
        /// </summary>
        /// <param name="predicate">Условие фильтрации</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Количество сущностей</returns>
        Task<int> CountAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default
        );
    }
}