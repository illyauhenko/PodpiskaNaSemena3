using PodpiskaNaSemena.Domain.Entities;

namespace PodpiskaNaSemena.Domain.Repositories.Abstractions
{
    /// <summary>
    /// Репозиторий для работы с отзывами на семена
    /// Управляет рейтингами и комментариями пользователей
    /// </summary>
    public interface IReviewRepository : IRepository<Review, int>
    {
        /// <summary>
        /// Получает все отзывы для указанных семян
        /// </summary>
        /// <param name="seedId">Идентификатор семян</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Коллекция отзывов на семена</returns>
        Task<IReadOnlyList<Review>> GetBySeedIdAsync(
            int seedId,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Вычисляет средний рейтинг для указанных семян
        /// </summary>
        /// <param name="seedId">Идентификатор семян</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Средний рейтинг от 1 до 5</returns>
        Task<double> GetAverageRatingAsync(
            int seedId,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Получает все отзывы, оставленные указанным пользователем
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Коллекция отзывов пользователя</returns>
        Task<IReadOnlyList<Review>> GetByUserIdAsync(
            int userId,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Проверяет оставлял ли пользователь отзыв на указанные семена
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="seedId">Идентификатор семян</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>True если отзыв уже существует</returns>
        Task<bool> HasUserReviewedSeedAsync(
            int userId,
            int seedId,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Получает последние отзывы в системе
        /// </summary>
        /// <param name="count">Количество отзывов для возврата</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Коллекция последних отзывов</returns>
        Task<IReadOnlyList<Review>> GetRecentReviewsAsync(
            int count,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Получает общее количество отзывов для указанных семян
        /// </summary>
        /// <param name="seedId">Идентификатор семян</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Количество отзывов</returns>
        Task<int> GetReviewCountForSeedAsync(
            int seedId,
            CancellationToken cancellationToken = default
        );
    }
}