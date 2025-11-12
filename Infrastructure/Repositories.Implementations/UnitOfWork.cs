using Microsoft.EntityFrameworkCore.Storage;
using PodpiskaNaSemena.Domain.Repositories.Abstractions;
using PodpiskaNaSemena3.Infrastructure.EntityFramework;
using PodpiskaNaSemena3.Infrastructure.Repositories.Implementations.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PodpiskaNaSemena3.Infrastructure.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _currentTransaction;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            Seeds = new SeedRepository(_context);
            Subscriptions = new SubscriptionRepository(_context);
            Payments = new PaymentRepository(_context);
            Reviews = new ReviewRepository(_context);
        }

        // === РЕПОЗИТОРИИ ===
        public IUserRepository Users { get; }
        public ISeedRepository Seeds { get; }
        public ISubscriptionRepository Subscriptions { get; }
        public IPaymentRepository Payments { get; }
        public IReviewRepository Reviews { get; }

        // === УПРАВЛЕНИЕ ТРАНЗАКЦИЯМИ ===

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null)
            {
                throw new InvalidOperationException("Транзакция уже начата");
            }

            _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("Нет активной транзакции для коммита");
            }

            try
            {
                await SaveChangesAsync(cancellationToken);
                await _currentTransaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("Нет активной транзакции для отката");
            }

            try
            {
                await _currentTransaction.RollbackAsync(cancellationToken);
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        // === СОСТОЯНИЕ СИСТЕМЫ ===

        public bool HasActiveTransaction => _currentTransaction != null;

        public bool IsDisposed { get; private set; }

        // === ДОПОЛНИТЕЛЬНЫЕ МЕТОДЫ ===

        public async Task<bool> CanConnectAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Database.CanConnectAsync(cancellationToken);
            }
            catch
            {
                return false;
            }
        }

        public async Task EnsureCreatedAsync(CancellationToken cancellationToken = default)
        {
            await _context.Database.EnsureCreatedAsync(cancellationToken);
        }

        public async Task EnsureDeletedAsync(CancellationToken cancellationToken = default)
        {
            await _context.Database.EnsureDeletedAsync(cancellationToken);
        }

        // === DISPOSE PATTERN ===

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    _currentTransaction?.Dispose();
                    _context?.Dispose();
                }

                _currentTransaction = null;
                IsDisposed = true;
            }
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }

            if (_context != null)
            {
                await _context.DisposeAsync();
            }
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}