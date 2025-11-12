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
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // === БАЗОВЫЕ CRUD МЕТОДЫ ===

        public async Task<Review?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Reviews
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<Review>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Reviews
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Review>> FindAsync(
            Expression<Func<Review, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await _context.Reviews
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Review entity, CancellationToken cancellationToken = default)
        {
            await _context.Reviews.AddAsync(entity, cancellationToken);
        }

        public async Task UpdateAsync(Review entity, CancellationToken cancellationToken = default)
        {
            _context.Reviews.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Review entity, CancellationToken cancellationToken = default)
        {
            _context.Reviews.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Reviews
                .AsNoTracking()
                .AnyAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Reviews
                .AsNoTracking()
                .CountAsync(cancellationToken);
        }

        public async Task<int> CountAsync(
            Expression<Func<Review, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await _context.Reviews
                .AsNoTracking()
                .CountAsync(predicate, cancellationToken);
        }

        // === СПЕЦИФИЧНЫЕ МЕТОДЫ ===

        public async Task<IReadOnlyList<Review>> GetBySeedIdAsync(
            int seedId, CancellationToken cancellationToken = default)
        {
            return await _context.Reviews
                .AsNoTracking()
                .Where(r => r.SeedId == seedId)
                .ToListAsync(cancellationToken);
        }

        public async Task<double> GetAverageRatingAsync(
            int seedId, CancellationToken cancellationToken = default)
        {
            var average = await _context.Reviews
                .AsNoTracking()
                .Where(r => r.SeedId == seedId)
                .AverageAsync(r => (double?)r.Rating.Value, cancellationToken);

            return average ?? 0.0;
        }

        public async Task<IReadOnlyList<Review>> GetByUserIdAsync(
            int userId, CancellationToken cancellationToken = default)
        {
            return await _context.Reviews
                .AsNoTracking()
                .Where(r => r.UserId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> HasUserReviewedSeedAsync(
            int userId, int seedId, CancellationToken cancellationToken = default)
        {
            return await _context.Reviews
                .AsNoTracking()
                .AnyAsync(r => r.UserId == userId && r.SeedId == seedId, cancellationToken);
        }

        public async Task<IReadOnlyList<Review>> GetRecentReviewsAsync(
            int count, CancellationToken cancellationToken = default)
        {
            return await _context.Reviews
                .AsNoTracking()
                .OrderByDescending(r => r.CreatedAt)
                .Take(count)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetReviewCountForSeedAsync(
            int seedId, CancellationToken cancellationToken = default)
        {
            return await _context.Reviews
                .AsNoTracking()
                .CountAsync(r => r.SeedId == seedId, cancellationToken);
        }
    }
}