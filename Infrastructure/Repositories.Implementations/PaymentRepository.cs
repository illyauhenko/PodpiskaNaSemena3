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
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Payment entity, CancellationToken cancellationToken = default)
        {
            await _context.Payments.AddAsync(entity, cancellationToken);
        }

        public async Task DeleteAsync(Payment entity, CancellationToken cancellationToken = default)
        {
            _context.Payments.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<IReadOnlyList<Payment>> FindAsync(System.Linq.Expressions.Expression<System.Func<Payment, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.Payments.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Payment>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Payments.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<Payment?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Payments.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<Payment?> GetBySubscriptionIdAsync(int subscriptionId, CancellationToken cancellationToken = default)
        {
            return await _context.Payments.AsNoTracking().FirstOrDefaultAsync(p => p.SubscriptionId == subscriptionId, cancellationToken);
        }

        public async Task UpdateAsync(Payment entity, CancellationToken cancellationToken = default)
        {
            _context.Payments.Update(entity);
            await Task.CompletedTask;
        }
    }
}
