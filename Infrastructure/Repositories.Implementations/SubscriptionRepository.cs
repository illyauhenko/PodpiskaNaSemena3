using Microsoft.EntityFrameworkCore;
using PodpiskaNaSemena.Domain.Entities;
using PodpiskaNaSemena.Domain.Repositories.Abstractions;
using PodpiskaNaSemena3.Infrastructure.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PodpiskaNaSemena3.Infrastructure.Repositories.Implementations.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Subscription entity, CancellationToken cancellationToken = default)
        {
            await _context.Subscriptions.AddAsync(entity, cancellationToken);
        }

        public async Task DeleteAsync(Subscription entity, CancellationToken cancellationToken = default)
        {
            _context.Subscriptions.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<IReadOnlyList<Subscription>> FindAsync(System.Linq.Expressions.Expression<System.Func<Subscription, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.Subscriptions.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Subscription>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Subscriptions.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<Subscription?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Subscriptions.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<IReadOnlyList<Subscription>> GetActiveSubscriptionsForUserAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _context.Subscriptions.AsNoTracking()
                .Where(s => s.UserId == userId && s.Status == PodpiskaNaSemena.Domain.Enums.SubscriptionStatus.Active)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> HasActiveSubscriptionAsync(int userId, int seedId, CancellationToken cancellationToken = default)
        {
            return await _context.Subscriptions.AsNoTracking()
                .AnyAsync(s => s.UserId == userId && s.SeedId == seedId && s.Status == PodpiskaNaSemena.Domain.Enums.SubscriptionStatus.Active, cancellationToken);
        }

        public async Task UpdateAsync(Subscription entity, CancellationToken cancellationToken = default)
        {
            _context.Subscriptions.Update(entity);
            await Task.CompletedTask;
        }
    }
}
