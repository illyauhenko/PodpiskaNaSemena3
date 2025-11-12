using PodpiskaNaSemena.Application.Models.Base;

namespace PodpiskaNaSemena.Application.Models.User
{
    /// <summary>
    /// Запрос на создание пользователя
    /// Используется при регистрации нового пользователя
    /// </summary>
    public sealed record CreateUserRequest(
        string Username,
        string Email
    ) : CreateRequestModel;
}