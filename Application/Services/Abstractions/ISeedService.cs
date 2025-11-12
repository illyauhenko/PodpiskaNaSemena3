using PodpiskaNaSemena.Application.Models.Seed;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PodpiskaNaSemena.Application.Services.Abstractions
{

    /// <summary>
    /// Сервис для управления каталогом семян
    /// Отвечает за CRUD операции с семенами и поиск по каталогу
    /// </summary>
    public interface ISeedService
    {
        /// <summary>
        /// Создает новые семена в каталоге
        /// Только администраторы могут создавать новые позиции
        /// </summary>
        /// <param name="request">Данные для создания семян</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>Созданные семена</returns>
        Task<SeedResponse> CreateSeedAsync(CreateSeedRequest request, CancellationToken ct = default);

        /// <summary>
        /// Обновляет информацию о существующих семенах
        /// Только администраторы могут изменять данные семян
        /// </summary>
        /// <param name="id">Идентификатор семян для обновления</param>
        /// <param name="request">Новые данные семян</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>Обновленные семена</returns>
        Task<SeedResponse> UpdateSeedAsync(int id, CreateSeedRequest request, CancellationToken ct = default);

        /// <summary>
        /// Получает основную информацию о семенах по идентификатору
        /// Используется для отображения в каталоге и карточках товара
        /// </summary>
        /// <param name="id">Идентификатор семян</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>Данные семян</returns>
        Task<SeedResponse> GetSeedAsync(int id, CancellationToken ct = default);

        /// <summary>
        /// Получает подробную информацию о семенах включая статистику
        /// Включает количество отзывов, подписок и средний рейтинг
        /// </summary>
        /// <param name="id">Идентификатор семян</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>Подробные данные семян</returns>
        Task<SeedDetailsResponse> GetSeedDetailsAsync(int id, CancellationToken ct = default);

        /// <summary>
        /// Получает семена вместе со всеми отзывами на них
        /// Используется для страницы товара где показываются отзывы
        /// </summary>
        /// <param name="id">Идентификатор семян</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>Семена с коллекцией отзывов</returns>
        Task<SeedWithReviewsResponse> GetSeedWithReviewsAsync(int id, CancellationToken ct = default);

        /// <summary>
        /// Ищет семена по названию или ключевым словам
        /// Использует поиск по подстроке в названии и описании
        /// </summary>
        /// <param name="keyword">Ключевое слово для поиска</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>Список найденных семян</returns>
        Task<IReadOnlyList<SeedResponse>> SearchSeedsAsync(string keyword, CancellationToken ct = default);

        /// <summary>
        /// Получает семена с наивысшим рейтингом
        /// Используется для отображения "топовых" семян на главной странице
        /// </summary>
        /// <param name="count">Количество семян для возврата</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>Список семян с высоким рейтингом</returns>
        Task<IReadOnlyList<SeedResponse>> GetTopRatedSeedsAsync(int count, CancellationToken ct = default);

        /// <summary>
        /// Получает все семена доступные для подписки
        /// Фильтрует семена с флагом IsAvailable = true
        /// </summary>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>Список доступных семян</returns>
        Task<IReadOnlyList<SeedResponse>> GetAvailableSeedsAsync(CancellationToken ct = default);
    }
}