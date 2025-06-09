using PodpiskaNaSemena.Domain.Repositories.Abstractions;
using PodpiskaNaSemena3.Infrastructure.EntityFramework;
using PodpiskaNaSemena3.Infrastructure.Repositories.Implementations.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace PodpiskaNaSemena3.Infrastructure.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            UserRepository = new UserRepository(_context);
            SeedRepository = new SeedRepository(_context);
            SubscriptionRepository = new SubscriptionRepository(_context);
            PaymentRepository = new PaymentRepository(_context);
            ReviewRepository = new ReviewRepository(_context);
        }

        public IUserRepository UserRepository { get; }
        public ISeedRepository SeedRepository { get; }
        public ISubscriptionRepository SubscriptionRepository { get; }
        public IPaymentRepository PaymentRepository { get; }
        public IReviewRepository ReviewRepository { get; }

        public Task BeginTransactionAsync(CancellationToken cancellationToken = default) =>
            _context.Database.BeginTransactionAsync(cancellationToken);

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
            await _context.Database.CommitTransactionAsync(cancellationToken);
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default) =>
            await _context.Database.RollbackTransactionAsync(cancellationToken);

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            await _context.SaveChangesAsync(cancellationToken);
    }
}
