using PodpiskaNaSemena.Domain.Entities.Base;
using PodpiskaNaSemena.Domain.ValueObjects;
using PodpiskaNaSemena.Domain.Exceptions;

namespace PodpiskaNaSemena.Domain.Entities
{
    public class User : Entity<int>
    {
        public Username Username { get; }
        public Email Email { get; }
        public DateTime CreatedAt { get; }
        public IReadOnlyCollection<Subscription> Subscriptions => _subscriptions.AsReadOnly();
        public IReadOnlyCollection<Review> Reviews => _reviews.AsReadOnly();

        private readonly List<Subscription> _subscriptions = new();
        private readonly List<Review> _reviews = new();

        // Конструктор для EF Core
        protected User() { }

        public User(int id, Username username, Email email) : base(id)
        {
            Username = username ?? throw new DomainException("Username обязателен");
            Email = email ?? throw new DomainException("Email обязателен");
            CreatedAt = DateTime.UtcNow;
        }
    }
}