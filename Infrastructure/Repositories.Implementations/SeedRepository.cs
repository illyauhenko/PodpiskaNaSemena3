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
    public class SeedRepository : ISeedRepository
    {
        private readonly ApplicationDbContext _context;

        public SeedRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Seed entity, CancellationToken cancellationToken = default)
        {
            await _context.Seeds.AddAsync(entity, cancellationToken);
        }

        public async Task DeleteAsync(Seed entity, CancellationToken cancellationToken = default)
        {
            _context.Seeds.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<IReadOnlyList<Seed>> FindAsync(System.Linq.Expressions.Expression<System.Func<Seed, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.Seeds.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Seed>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Seeds.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<Seed?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Seeds.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<IReadOnlyList<Seed>> GetTopRatedAsync(int count, CancellationToken cancellationToken = default)
        {
            // Здесь простой пример, реальная логика может быть сложнее (возможно нужно джоинить Reviews и сортировать)
            return await _context.Seeds.AsNoTracking()
                .OrderByDescending(s => s.Reviews.Any() ? s.Reviews.Average(r => r.Rating.Value) : 0)
                .Take(count)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Seed>> SearchByNameAsync(string keyword, CancellationToken cancellationToken = default)
        {
            return await _context.Seeds.AsNoTracking()
                .Where(s => EF.Functions.ILike(s.Name.Value, $"%{keyword}%"))
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(Seed entity, CancellationToken cancellationToken = default)
        {
            _context.Seeds.Update(entity);
            await Task.CompletedTask;
        }
    }
}
