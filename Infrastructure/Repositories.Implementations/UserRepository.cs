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
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // === БАЗОВЫЕ CRUD МЕТОДЫ ===

        public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .Include(u => u.Subscriptions)
                .Include(u => u.Reviews)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .AsNoTracking()
                .Include(u => u.Subscriptions)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<User>> FindAsync(
            Expression<Func<User, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(predicate)
                .Include(u => u.Subscriptions)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(User entity, CancellationToken cancellationToken = default)
        {
            await _context.Users.AddAsync(entity, cancellationToken);
        }


        public async Task UpdateAsync(User entity, CancellationToken cancellationToken = default)
        {
            _context.Users.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(User entity, CancellationToken cancellationToken = default)
        {
            _context.Users.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .AsNoTracking()
                .AnyAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .AsNoTracking()
                .CountAsync(cancellationToken);
        }

        public async Task<int> CountAsync(
            Expression<Func<User, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .AsNoTracking()
                .CountAsync(predicate, cancellationToken);
        }

        // === СПЕЦИФИЧНЫЕ МЕТОДЫ ===

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .AsNoTracking()
                .Include(u => u.Subscriptions)
                .FirstOrDefaultAsync(u => u.Email.Value == email, cancellationToken);
        }

        public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default)
        {
            return !await _context.Users
                .AsNoTracking()
                .AnyAsync(u => u.Email.Value == email, cancellationToken);
        }

        public async Task<IReadOnlyList<User>> GetUsersWithExpiringSubscriptionsAsync(
            int daysThreshold, CancellationToken cancellationToken = default)
        {
            var expirationDate = DateTime.UtcNow.AddDays(daysThreshold);

            return await _context.Users
                .AsNoTracking()
                .Include(u => u.Subscriptions)
                .Where(u => u.Subscriptions.Any(s =>
                    s.Status == PodpiskaNaSemena.Domain.Enums.SubscriptionStatus.Active &&
                    s.EndDate <= expirationDate))
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> IsAdminAsync(int userId, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            return user?.IsAdmin ?? false;
        }

        public async Task<IReadOnlyList<User>> GetAdminsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.IsAdmin)
                .ToListAsync(cancellationToken);
        }
        //  КОНЕЦ Исправления :

        public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .AsNoTracking()
                .Include(u => u.Subscriptions)
                .FirstOrDefaultAsync(u => u.Username.Value == username, cancellationToken);
        }
    }
}