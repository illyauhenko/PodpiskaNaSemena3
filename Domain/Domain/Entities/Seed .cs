using PodpiskaNaSemena.Domain.Entities.Base;
using PodpiskaNaSemena.Domain.Exceptions;
using PodpiskaNaSemena.Domain.ValueObjects;

namespace PodpiskaNaSemena.Domain.Entities
{
    public class Seed : Entity<int>
    {
        public SeedName Name { get; }
        public Description Description { get; }
        public Price Price { get; }
        public IReadOnlyCollection<Subscription> Subscriptions => _subscriptions.AsReadOnly();
        public IReadOnlyCollection<Review> Reviews => _reviews.AsReadOnly();

        private readonly List<Subscription> _subscriptions = new();
        private readonly List<Review> _reviews = new();

        protected Seed() { }

        public Seed(int id, SeedName name, Description description, Price price) : base(id)
        {
            Name = name ?? throw new DomainException("Название семени обязательно");
            Description = description ?? throw new DomainException("Описание обязательно");
            Price = price ?? throw new DomainException("Цена обязательна");
        }
    }
}