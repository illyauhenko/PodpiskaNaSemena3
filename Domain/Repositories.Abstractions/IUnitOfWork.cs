namespace PodpiskaNaSemena.Domain.Repositories.Abstractions
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        ISeedRepository SeedRepository { get; }
        ISubscriptionRepository SubscriptionRepository { get; }
        IPaymentRepository PaymentRepository { get; }
        IReviewRepository ReviewRepository { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}