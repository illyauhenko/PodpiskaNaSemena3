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
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // === БАЗОВЫЕ CRUD МЕТОДЫ ===

        public async Task<Payment?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<Payment>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Payments
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Payment>> FindAsync(
            Expression<Func<Payment, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await _context.Payments
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Payment entity, CancellationToken cancellationToken = default)
        {
            await _context.Payments.AddAsync(entity, cancellationToken);
        }

        public async Task UpdateAsync(Payment entity, CancellationToken cancellationToken = default)
        {
            _context.Payments.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Payment entity, CancellationToken cancellationToken = default)
        {
            _context.Payments.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Payments
                .AsNoTracking()
                .AnyAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Payments
                .AsNoTracking()
                .CountAsync(cancellationToken);
        }

        public async Task<int> CountAsync(
            Expression<Func<Payment, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await _context.Payments
                .AsNoTracking()
                .CountAsync(predicate, cancellationToken);
        }

        // === СПЕЦИФИЧНЫЕ МЕТОДЫ ===

        public async Task<Payment?> GetBySubscriptionIdAsync(
            int subscriptionId, CancellationToken cancellationToken = default)
        {
            return await _context.Payments
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.SubscriptionId == subscriptionId, cancellationToken);
        }

        public async Task<IReadOnlyList<Payment>> GetPendingPaymentsAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.Payments
                .AsNoTracking()
                .Where(p => p.Status == PodpiskaNaSemena.Domain.Enums.PaymentStatus.Pending)
                .ToListAsync(cancellationToken);
        }

        public async Task<Payment?> GetLatestBySubscriptionIdAsync(
            int subscriptionId, CancellationToken cancellationToken = default)
        {
            return await _context.Payments
                .AsNoTracking()
                .Where(p => p.SubscriptionId == subscriptionId)
                .OrderByDescending(p => p.PaymentDate)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Payment>> GetPaymentsByStatusAsync(
            PodpiskaNaSemena.Domain.Enums.PaymentStatus status, CancellationToken cancellationToken = default)
        {
            return await _context.Payments
                .AsNoTracking()
                .Where(p => p.Status == status)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Payment>> GetPaymentsByDateRangeAsync(
            DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _context.Payments
                .AsNoTracking()
                .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
                .OrderBy(p => p.PaymentDate)
                .ToListAsync(cancellationToken);
        }
    }
}