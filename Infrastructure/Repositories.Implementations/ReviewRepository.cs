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
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Review entity, CancellationToken cancellationToken = default)
        {
            await _context.Reviews.AddAsync(entity, cancellationToken);
        }

        public async Task DeleteAsync(Review entity, CancellationToken cancellationToken = default)
        {
            _context.Reviews.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<IReadOnlyList<Review>> FindAsync(System.Linq.Expressions.Expression<System.Func<Review, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.Reviews.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Review>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Reviews.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<Review?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Reviews.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<IReadOnlyList<Review>> GetBySeedIdAsync(int seedId, CancellationToken cancellationToken = default)
        {
            return await _context.Reviews.AsNoTracking()
                                         .Where(r => r.SeedId == seedId)
                                         .ToListAsync(cancellationToken);
        }

        public async Task<double> GetAverageRatingAsync(int seedId, CancellationToken cancellationToken = default)
        {
            var ratings = await _context.Reviews.AsNoTracking()
                                                .Where(r => r.SeedId == seedId)
                                                .Select(r => (double?)r.Rating.Value)
                                                .ToListAsync(cancellationToken);
            return ratings.Count == 0 ? 0.0 : ratings.Average() ?? 0.0;
        }

        public async Task UpdateAsync(Review entity, CancellationToken cancellationToken = default)
        {
            _context.Reviews.Update(entity);
            await Task.CompletedTask;
        }
    }
}
