using PodpiskaNaSemena.Domain.Entities;
using PodpiskaNaSemena.Domain.Repositories.Abstractions;

namespace PodpiskaNaSemena.Domain.Repositories.Abstractions
{
    public interface IPaymentRepository : IRepository<Payment, int>
    {
        Task<Payment?> GetBySubscriptionIdAsync(
            int subscriptionId,
            CancellationToken cancellationToken = default
        );
    }
}