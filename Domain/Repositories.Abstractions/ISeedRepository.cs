using PodpiskaNaSemena.Domain.Entities;
using PodpiskaNaSemena.Domain.Repositories.Abstractions;

namespace PodpiskaNaSemena.Domain.Repositories.Abstractions
{
    public interface ISeedRepository : IRepository<Seed, int>
    {
        Task<IReadOnlyList<Seed>> GetTopRatedAsync(int count, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Seed>> SearchByNameAsync(string keyword, CancellationToken cancellationToken = default);
    }
}