namespace PodpiskaNaSemena.Application.Models.User
{
    public sealed record UserCreateModel(
        string Username,
        string Email
    );
}