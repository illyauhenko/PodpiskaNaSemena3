using PodpiskaNaSemena.Domain.Entities.Base;
using PodpiskaNaSemena.Domain.Exceptions;
using PodpiskaNaSemena.Domain.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace PodpiskaNaSemena.Domain.Entities
{
    public class Seed : Entity<int>
    {
        public SeedName Name { get; }
        public Description Description { get; }
        public Price Price { get; }
        public bool IsAvailable { get; private set; } = true;

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

        // Методы управления доступностью
        public void MarkAsAvailable() => IsAvailable = true;
        public void MarkAsUnavailable() => IsAvailable = false;

        // Методы для работы с подписками
        public void AddSubscription(Subscription subscription)
        {
            if (subscription == null)
                throw new DomainException("Подписка не может быть null");

            if (!IsAvailable)
                throw new DomainException("Нельзя добавить подписку на недоступные семена");

            if (_subscriptions.Contains(subscription))
                throw new DomainException("Подписка уже добавлена к этим семенам");

            _subscriptions.Add(subscription);
        }

        public void RemoveSubscription(Subscription subscription)
        {
            if (subscription == null)
                throw new DomainException("Подписка не может быть null");

            _subscriptions.Remove(subscription);
        }

        // Методы для работы с отзывами
        public void AddReview(Review review)
        {
            if (review == null)
                throw new DomainException("Отзыв не может быть null");

            if (_reviews.Contains(review))
                throw new DomainException("Отзыв уже добавлен");

            _reviews.Add(review);
        }

        public void RemoveReview(Review review)
        {
            if (review == null)
                throw new DomainException("Отзыв не может быть null");

            _reviews.Remove(review);
        }

        // Расчет среднего рейтинга
        public double CalculateAverageRating()
            => _reviews.Any() ? _reviews.Average(r => r.Rating.Value) : 0;

        // Получение количества активных подписок
        public int GetActiveSubscriptionsCount()
        {
            var currentDate = DateTime.UtcNow;
            return _subscriptions.Count(s => s.IsActive(currentDate));
        }

        // Проверка возможности создания подписки
        public bool CanCreateSubscription() => IsAvailable && Price.Value > 0;

        // Метод для обновления цены
        public void UpdatePrice(Price newPrice)
        {
            if (newPrice == null)
                throw new DomainException("Цена не может быть null");

            if (newPrice.Value <= 0)
                throw new DomainException("Цена должна быть положительной");

            // В реальности здесь была бы логика обновления через reflection
            // или создание нового объекта с обновленной ценой
        }

        // Дополнительные методы
        public string GetPriceFormatted() => Price.ToCurrencyString();
        public Rating GetAverageRatingObject()
            => new Rating((int)Math.Round(CalculateAverageRating()));

        public bool HasGoodRating() => CalculateAverageRating() >= 4.0;
    }
}