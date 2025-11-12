using PodpiskaNaSemena.Application.Models.Base;

namespace PodpiskaNaSemena.Application.Models.User
{
    public sealed record UpdateUserRequest(
        string? Username,
        string? Email
    ) : UpdateRequestModel<int>(0)
    {

    }; // ID будет устанавливаться отдельно
}