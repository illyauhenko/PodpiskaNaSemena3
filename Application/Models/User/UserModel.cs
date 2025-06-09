using PodpiskaNaSemena.Application.Models.Base;

namespace PodpiskaNaSemena.Application.Models.User
{
    public sealed record UserModel(
        int Id,
        string Username,
        string Email,
        DateTime CreatedAt
    ) : Model(Id, Username);
}