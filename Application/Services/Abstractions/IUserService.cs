using PodpiskaNaSemena.Application.Models.User;

public interface IUserService
{
    Task<UserModel> CreateAsync(UserCreateModel createModel, CancellationToken ct = default);
    Task<UserModel> GetByIdAsync(int id, CancellationToken ct = default);
}