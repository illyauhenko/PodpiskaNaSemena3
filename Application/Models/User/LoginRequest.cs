namespace PodpiskaNaSemena.Application.Models.User
{
    /// <summary>
    /// Запрос на вход в систему
    /// </summary>
    public sealed record LoginRequest(
        string Username,
        string Password
    );
}