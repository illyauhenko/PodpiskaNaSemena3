using Microsoft.EntityFrameworkCore;
using PodpiskaNaSemena.Domain.Entities;
using PodpiskaNaSemena.Domain.Repositories.Abstractions;

using PodpiskaNaSemena3.Infrastructure.EntityFramework;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PodpiskaNaSemena3.Infrastructure.Repositories.Implementations.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User entity, CancellationToken cancellationToken = default)
        {
            await _context.Users.AddAsync(entity, cancellationToken);
        }

        public async Task DeleteAsync(User entity, CancellationToken cancellationToken = default)
        {
            _context.Users.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<IReadOnlyList<User>> FindAsync(System.Linq.Expressions.Expression<System.Func<User, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.Users.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Users.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Users.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.Value == email, cancellationToken);
        }

        public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default)
        {
            return !await _context.Users.AsNoTracking()
                .AnyAsync(u => u.Email.Value == email, cancellationToken);
        }

        public async Task UpdateAsync(User entity, CancellationToken cancellationToken = default)
        {
            _context.Users.Update(entity);
            await Task.CompletedTask;
        }

        public Task GetByIdAsync(Guid userId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
