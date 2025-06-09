using PodpiskaNaSemena.Domain.Entities;
using PodpiskaNaSemena.Domain.Enums;
using PodpiskaNaSemena.Domain.Repositories.Abstractions;

namespace PodpiskaNaSemena.Domain.Repositories.Abstractions
{
    public interface ISubscriptionRepository : IRepository<Subscription, int>
    {
        Task<IReadOnlyList<Subscription>> GetActiveSubscriptionsForUserAsync(
            int userId,
            CancellationToken cancellationToken = default
        );

        Task<bool> HasActiveSubscriptionAsync(
            int userId,
            int seedId,
            CancellationToken cancellationToken = default
        );
    }
}