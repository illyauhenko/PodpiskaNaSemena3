using Microsoft.EntityFrameworkCore;
using PodpiskaNaSemena.Domain.Entities;
using PodpiskaNaSemena.Domain.Repositories.Abstractions;
using PodpiskaNaSemena3.Infrastructure.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        // === БАЗОВЫЕ CRUD МЕТОДЫ ===

        public async Task<Subscription?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Subscriptions
                .Include(s => s.Users)
                .Include(s => s.Payment)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<Subscription>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Subscriptions
                .AsNoTracking()
                .Include(s => s.Users)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Subscription>> FindAsync(
            Expression<Func<Subscription, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await _context.Subscriptions
                .AsNoTracking()
                .Where(predicate)
                .Include(s => s.Users)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Subscription entity, CancellationToken cancellationToken = default)
        {
            await _context.Subscriptions.AddAsync(entity, cancellationToken);
        }

        public async Task UpdateAsync(Subscription entity, CancellationToken cancellationToken = default)
        {
            _context.Subscriptions.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Subscription entity, CancellationToken cancellationToken = default)
        {
            _context.Subscriptions.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Subscriptions
                .AsNoTracking()
                .AnyAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Subscriptions
                .AsNoTracking()
                .CountAsync(cancellationToken);
        }

        public async Task<int> CountAsync(
            Expression<Func<Subscription, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await _context.Subscriptions
                .AsNoTracking()
                .CountAsync(predicate, cancellationToken);
        }

        // === СПЕЦИФИЧНЫЕ МЕТОДЫ ===

        public async Task<IReadOnlyList<Subscription>> GetActiveSubscriptionsForUserAsync(
            int userId, CancellationToken cancellationToken = default)
        {
            var currentDate = DateTime.UtcNow;

            return await _context.Subscriptions
                .AsNoTracking()
                .Include(s => s.Users)
                .Where(s => s.Users.Any(u => u.Id == userId) &&
                           s.Status == PodpiskaNaSemena.Domain.Enums.SubscriptionStatus.Active &&
                           s.StartDate <= currentDate &&
                           s.EndDate >= currentDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> HasActiveSubscriptionAsync(
            int userId, int seedId, CancellationToken cancellationToken = default)
        {
            var currentDate = DateTime.UtcNow;

            return await _context.Subscriptions
                .AsNoTracking()
                .Include(s => s.Users)
                .AnyAsync(s => s.Users.Any(u => u.Id == userId) &&
                              s.SeedId == seedId &&
                              s.Status ==   PodpiskaNaSemena.Domain.Enums.SubscriptionStatus.Active &&
                              s.StartDate <= currentDate &&
                              s.EndDate >= currentDate,
                          cancellationToken);
        }

        public async Task<IReadOnlyList<Subscription>> GetExpiringSubscriptionsAsync(
            int daysThreshold, CancellationToken cancellationToken = default)
        {
            var expirationDate = DateTime.UtcNow.AddDays(daysThreshold);
            var currentDate = DateTime.UtcNow;

            return await _context.Subscriptions
                .AsNoTracking()
                .Include(s => s.Users)
                .Where(s => s.Status == PodpiskaNaSemena.Domain.Enums.SubscriptionStatus.Active &&
                           s.EndDate <= expirationDate &&
                           s.EndDate >= currentDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Subscription>> GetSubscriptionsByStatusAsync(
            PodpiskaNaSemena.Domain.Enums.SubscriptionStatus status, CancellationToken cancellationToken = default)
        {
            return await _context.Subscriptions
                .AsNoTracking()
                .Include(s => s.Users)
                .Where(s => s.Status == status)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Subscription>> GetSubscriptionsBySeedAsync(
            int seedId, CancellationToken cancellationToken = default)
        {
            return await _context.Subscriptions
                .AsNoTracking()
                .Include(s => s.Users)
                .Where(s => s.SeedId == seedId)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetActiveSubscriptionsCountAsync(
            int seedId, CancellationToken cancellationToken = default)
        {
            var currentDate = DateTime.UtcNow;

            return await _context.Subscriptions
                .AsNoTracking()
                .CountAsync(s => s.SeedId == seedId &&
                                s.Status == PodpiskaNaSemena.Domain.Enums.SubscriptionStatus.Active &&
                                s.StartDate <= currentDate &&
                                s.EndDate >= currentDate,
                          cancellationToken);
        }
    }
}