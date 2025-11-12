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
    public class SeedRepository : ISeedRepository
    {
        private readonly ApplicationDbContext _context;

        public SeedRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // === БАЗОВЫЕ CRUD МЕТОДЫ ИЗ IRepository ===

        public async Task<Seed?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Seeds
                .Include(s => s.Subscriptions)
                .Include(s => s.Reviews)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<Seed>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Seeds
                .AsNoTracking()
                .Include(s => s.Reviews)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Seed>> FindAsync(
            Expression<Func<Seed, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await _context.Seeds
                .AsNoTracking()
                .Where(predicate)
                .Include(s => s.Reviews)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Seed entity, CancellationToken cancellationToken = default)
        {
            await _context.Seeds.AddAsync(entity, cancellationToken);
        }

        public async Task UpdateAsync(Seed entity, CancellationToken cancellationToken = default)
        {
            _context.Seeds.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Seed entity, CancellationToken cancellationToken = default)
        {
            _context.Seeds.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Seeds
                .AsNoTracking()
                .AnyAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Seeds
                .AsNoTracking()
                .CountAsync(cancellationToken);
        }

        public async Task<int> CountAsync(
            Expression<Func<Seed, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await _context.Seeds
                .AsNoTracking()
                .CountAsync(predicate, cancellationToken);
        }

        // === СПЕЦИФИЧНЫЕ МЕТОДЫ ИЗ ISeedRepository ===

        public async Task<IReadOnlyList<Seed>> GetTopRatedAsync(int count, CancellationToken cancellationToken = default)
        {
            return await _context.Seeds
                .AsNoTracking()
                .Include(s => s.Reviews)
                .Where(s => s.Reviews.Any())
                .OrderByDescending(s => s.Reviews.Average(r => r.Rating.Value))
                .Take(count)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Seed>> SearchByNameAsync(string keyword, CancellationToken cancellationToken = default)
        {
            return await _context.Seeds
                .AsNoTracking()
                .Where(s => EF.Functions.ILike(s.Name.Value, $"%{keyword}%"))
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Seed>> GetAvailableSeedsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Seeds
                .AsNoTracking()
                .Where(s => s.IsAvailable)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Seed>> GetSeedsByCategoryAsync(string category, CancellationToken cancellationToken = default)
        {
           
             return new List<Seed>().AsReadOnly();
        }

        public async Task<IReadOnlyList<Seed>> GetPopularSeedsAsync(int count, CancellationToken cancellationToken = default)
        {
            return await _context.Seeds
                .AsNoTracking()
                .Include(s => s.Subscriptions)
                .OrderByDescending(s => s.Subscriptions.Count(sb => sb.Status == PodpiskaNaSemena.Domain.Enums.SubscriptionStatus.Active))
                .Take(count)
                .ToListAsync(cancellationToken);
        }
    }
}