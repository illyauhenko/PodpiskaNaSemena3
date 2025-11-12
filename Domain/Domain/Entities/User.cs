using PodpiskaNaSemena.Domain.Entities.Base;
using PodpiskaNaSemena.Domain.Exceptions;
using PodpiskaNaSemena.Domain.ValueObjects;
using PodpiskaNaSemena3.Domain.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace PodpiskaNaSemena.Domain.Entities
{
    public class User : Entity<int>
    {
        public Username Username { get; }
        public Email Email { get; }
        public DateTime CreatedAt { get; }
        public bool IsAdmin { get; private set; }

        public IReadOnlyCollection<Subscription> Subscriptions => _subscriptions.AsReadOnly();
        public IReadOnlyCollection<Review> Reviews => _reviews.AsReadOnly();

        private readonly List<Subscription> _subscriptions = new();
        private readonly List<Review> _reviews = new();

        protected User() { }

        public User(int id, Username username, Email email) : base(id)
        {
            Username = username ?? throw new DomainException("Username обязателен");
            Email = email ?? throw new DomainException("Email обязателен");
            CreatedAt = DateTime.UtcNow;
            IsAdmin = false;
        }

        // Методы для работы с подписками
        public void SubscribeTo(Subscription subscription)
        {
            if (subscription == null)
                throw new DomainException("Подписка не может быть null");

            if (_subscriptions.Contains(subscription))
                throw new DomainException("Пользователь уже подписан на эту подписку");

            _subscriptions.Add(subscription);
            subscription.AddUser(this);
        }

        public void UnsubscribeFrom(Subscription subscription)
        {
            if (subscription == null)
                throw new DomainException("Подписка не может быть null");

            _subscriptions.Remove(subscription);
            subscription.RemoveUser(this);
        }

        // Методы управления правами
        public void MakeAdmin() => IsAdmin = true;
        public void RemoveAdmin() => IsAdmin = false;

        // Проверки прав
        public bool CanViewSubscription(Subscription subscription)
            => _subscriptions.Contains(subscription) || IsAdmin;

        public bool CanUseSubscription(Subscription subscription)
            => _subscriptions.Contains(subscription) &&
               subscription.IsActive(DateTime.UtcNow) &&
               subscription.Payment?.Status == Enums.PaymentStatus.Completed;

        public void CheckAdminRights()
        {
            if (!IsAdmin)
                throw new AdminRequiredException();
        }

        public void CheckSubscriptionAccess(Subscription subscription)
        {
            if (!CanViewSubscription(subscription))
                throw new SubscriptionAccessDeniedException(Id, subscription.Id);
        }

        public void CheckSubscriptionUsage(Subscription subscription)
        {
            if (!_subscriptions.Contains(subscription))
                throw new SubscriptionAccessDeniedException(Id, subscription.Id);

            if (subscription.Payment?.Status != Enums.PaymentStatus.Completed)
                throw new SubscriptionNotPaidException(subscription.Id);

            if (!subscription.IsActive(DateTime.UtcNow))
                throw new SubscriptionValidationException("Подписка не активна");
        }

        // Дополнительные методы
        public string GetEmailDomain() => Email.GetDomain();
        public bool HasCorporateEmail() => Email.IsCorporateEmail();
    }
}