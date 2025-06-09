using PodpiskaNaSemena.Domain.Entities;


namespace PodpiskaNaSemena.Domain.Repositories.Abstractions
{
    public interface IUserRepository : IRepository<User, int>
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task GetByIdAsync(Guid userId, CancellationToken ct);
        Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default);
    }
}