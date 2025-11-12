using PodpiskaNaSemena.Domain.Entities;

namespace PodpiskaNaSemena.Domain.Repositories.Abstractions
{
    /// <summary>
    /// Репозиторий для работы с пользователями системы
    /// Содержит специфичные методы для бизнес-логики пользователей
    /// </summary>
    public interface IUserRepository : IRepository<User, int>
    {
        /// <summary>
        /// Находит пользователя по email адресу
        /// </summary>
        /// <param name="email">Email для поиска</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Найденный пользователь или null</returns>
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Проверяет уникальность email адреса в системе
        /// </summary>
        /// <param name="email">Email для проверки</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>True если email уникален, иначе False</returns>
        Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получает пользователей с истекающими подписками
        /// Используется для отправки уведомлений о продлении
        /// </summary>
        /// <param name="daysThreshold">Количество дней до истечения подписки</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Коллекция пользователей</returns>
        Task<IReadOnlyList<User>> GetUsersWithExpiringSubscriptionsAsync(
            int daysThreshold,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Проверяет имеет ли пользователь права администратора
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>True если пользователь администратор</returns>
        Task<bool> IsAdminAsync(int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получает всех администраторов системы
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Коллекция пользователей-администраторов</returns>
        Task<IReadOnlyList<User>> GetAdminsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Находит пользователя по username
        /// </summary>
        Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    }
}