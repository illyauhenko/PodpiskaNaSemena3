using PodpiskaNaSemena.Application.Models.Base;

namespace PodpiskaNaSemena.Application.Models.User
{
    /// <summary>
    /// Ответ с данными о пользователе
    /// Используется для отображения информации о пользователе
    /// </summary>
    public sealed record UserResponse(
        int Id,
        string Username,
        string Email,
        DateTime CreatedAt
    ) : ResponseModel<int>(Id);
}