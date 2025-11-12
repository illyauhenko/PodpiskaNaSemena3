using PodpiskaNaSemena.Domain.Entities;

namespace PodpiskaNaSemena.Domain.Repositories.Abstractions
{
    /// <summary>
    /// Репозиторий для работы с каталогом семян
    /// Управляет товарными позициями системы
    /// </summary>
    public interface ISeedRepository : IRepository<Seed, int>
    {
        /// <summary>
        /// Получает семена с наивысшим рейтингом
        /// </summary>
        /// <param name="count">Количество семян для возврата</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Коллекция самых рейтинговых семян</returns>
        Task<IReadOnlyList<Seed>> GetTopRatedAsync(
            int count,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Ищет семена по названию (поиск по ключевым словам)
        /// </summary>
        /// <param name="keyword">Ключевое слово для поиска</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Коллекция найденных семян</returns>
        Task<IReadOnlyList<Seed>> SearchByNameAsync(
            string keyword,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Получает все доступные для подписки семена
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Коллекция доступных семян</returns>
        Task<IReadOnlyList<Seed>> GetAvailableSeedsAsync(
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Получает семена по категории (овощи, цветы, травы и т.д.)
        /// </summary>
        /// <param name="category">Категория семян</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Коллекция семян указанной категории</returns>
        Task<IReadOnlyList<Seed>> GetSeedsByCategoryAsync(
            string category,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Получает самые популярные семена (по количеству подписок)
        /// </summary>
        /// <param name="count">Количество семян для возврата</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Коллекция популярных семян</returns>
        Task<IReadOnlyList<Seed>> GetPopularSeedsAsync(
            int count,
            CancellationToken cancellationToken = default
        );
    }
}