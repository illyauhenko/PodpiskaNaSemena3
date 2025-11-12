using PodpiskaNaSemena.Application.Models.Review;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PodpiskaNaSemena.Application.Services.Abstractions
{

    /// <summary>
    /// Сервис для управления отзывами и рейтингами семян
    /// Отвечает за создание, редактирование и модерацию отзывов пользователей
    /// </summary>
    public interface IReviewService
    {
        /// <summary>
        /// Создает новый отзыв на семена
        /// Проверяет что пользователь имеет активную подписку на эти семена
        /// </summary>
        /// <param name="request">Данные для создания отзыва</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>Созданный отзыв</returns>
        Task<ReviewResponse> CreateReviewAsync(CreateReviewRequest request, CancellationToken ct = default);

        /// <summary>
        /// Обновляет существующий отзыв
        /// Разрешает редактирование только автору отзыва или администратору
        /// </summary>
        /// <param name="reviewId">Идентификатор отзыва для обновления</param>
        /// <param name="request">Новые данные отзыва</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>Обновленный отзыв</returns>
        Task<ReviewResponse> UpdateReviewAsync(int reviewId, CreateReviewRequest request, CancellationToken ct = default);

        /// <summary>
        /// Удаляет отзыв из системы
        /// Разрешает удаление только автору отзыва или администратору
        /// </summary>
        /// <param name="reviewId">Идентификатор отзыва для удаления</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        Task DeleteReviewAsync(int reviewId, CancellationToken ct = default);

        /// <summary>
        /// Получает отзыв по идентификатору
        /// Используется для отображения конкретного отзыва
        /// </summary>
        /// <param name="id">Идентификатор отзыва</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>Данные отзыва</returns>
        Task<ReviewResponse> GetReviewAsync(int id, CancellationToken ct = default);

        /// <summary>
        /// Получает все отзывы для указанных семян
        /// Используется для отображения отзывов на странице семян
        /// </summary>
        /// <param name="seedId">Идентификатор семян</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>Список отзывов на семена</returns>
        Task<IReadOnlyList<ReviewResponse>> GetReviewsForSeedAsync(int seedId, CancellationToken ct = default);

        /// <summary>
        /// Получает все отзывы указанного пользователя
        /// Используется для отображения в личном кабинете пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>Список отзывов пользователя</returns>
        Task<IReadOnlyList<ReviewResponse>> GetUserReviewsAsync(int userId, CancellationToken ct = default);

        /// <summary>
        /// Вычисляет средний рейтинг для указанных семян
        /// Используется для отображения звездочек рейтинга в каталоге
        /// </summary>
        /// <param name="seedId">Идентификатор семян</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>Средний рейтинг от 1.0 до 5.0</returns>
        Task<double> GetAverageRatingAsync(int seedId, CancellationToken ct = default);

        /// <summary>
        /// Проверяет оставлял ли пользователь отзыв на указанные семена
        /// Используется для предотвращения дублирования отзывов
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="seedId">Идентификатор семян</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>True если пользователь уже оставлял отзыв на эти семена</returns>
        Task<bool> HasUserReviewedSeedAsync(int userId, int seedId, CancellationToken ct = default);
    }
}