using PodpiskaNaSemena.Application.Models.User;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PodpiskaNaSemena.Application.Services.Abstractions
{

    /// <summary>
    /// Сервис для управления пользователями системы
    /// Отвечает за регистрацию, аутентификацию и управление профилями пользователей
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Создает нового пользователя в системе
        /// Используется при регистрации новых садоводов
        /// </summary>
        /// <param name="request">Данные для создания пользователя</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>Созданный пользователь</returns>
        Task<UserResponse> CreateUserAsync(CreateUserRequest request, CancellationToken ct = default);

        /// <summary>
        /// Получает основную информацию о пользователе по идентификатору
        /// Используется для отображения профиля пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>Данные пользователя</returns>
        Task<UserResponse> GetUserAsync(int id, CancellationToken ct = default);

        /// <summary>
        /// Получает подробную информацию о пользователе включая статистику
        /// Используется в админ-панели и для детального просмотра профиля
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>Подробные данные пользователя со статистикой</returns>
        Task<UserDetailsResponse> GetUserDetailsAsync(int id, CancellationToken ct = default);

        /// <summary>
        /// Назначает пользователю права администратора
        /// Только существующие администраторы могут выполнять эту операцию
        /// </summary>
        /// <param name="userId">Идентификатор пользователя для назначения прав</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        Task MakeUserAdminAsync(int userId, CancellationToken ct = default);

        /// <summary>
        /// Снимает права администратора с пользователя
        /// Только существующие администраторы могут выполнять эту операцию
        /// </summary>
        /// <param name="userId">Идентификатор пользователя для снятия прав</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        Task RemoveUserAdminAsync(int userId, CancellationToken ct = default);

        /// <summary>
        /// Находит пользователя по email адресу
        /// Используется для входа в систему и восстановления пароля
        /// </summary>
        /// <param name="email">Email адрес для поиска</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>Найденный пользователь или null если не найден</returns>
        Task<UserResponse?> GetUserByEmailAsync(string email, CancellationToken ct = default);

        /// <summary>
        /// Проверяет уникальность email адреса в системе
        /// Используется при регистрации для валидации
        /// </summary>
        /// <param name="email">Email для проверки</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>True если email уникален, иначе False</returns>
        Task<bool> IsEmailUniqueAsync(string email, CancellationToken ct = default);

        /// <summary>
        /// Получает всех пользователей (для админа)
        /// </summary>
        Task<IReadOnlyList<UserResponse>> GetAllUsersAsync(CancellationToken ct = default);

        /// <summary>
        /// Удаляет пользователя
        /// </summary>
        Task DeleteUserAsync(int userId, CancellationToken ct = default);

        /// <summary>
        /// Находит пользователя по username
        /// </summary>
        Task<UserResponse?> GetUserByUsernameAsync(string username, CancellationToken ct = default);
        // В IUserService.cs ДОБАВИТЬ:

        /// <summary>
        /// Обновляет данные пользователя
        /// </summary>
        Task<UserResponse> UpdateUserAsync(int id, UpdateUserRequest request, CancellationToken ct = default);

    }
}