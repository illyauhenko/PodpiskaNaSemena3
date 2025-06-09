using PodpiskaNaSemena.Domain.Entities;
using PodpiskaNaSemena.Domain.Repositories.Abstractions;

namespace PodpiskaNaSemena.Domain.Repositories.Abstractions
{
    public interface IReviewRepository : IRepository<Review, int>
    {
        Task<IReadOnlyList<Review>> GetBySeedIdAsync(
            int seedId,
            CancellationToken cancellationToken = default
        );

        Task<double> GetAverageRatingAsync(
            int seedId,
            CancellationToken cancellationToken = default
        );
    }
}