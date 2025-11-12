using PodpiskaNaSemena.Domain.Entities.Base;
using PodpiskaNaSemena.Domain.Exceptions;
using PodpiskaNaSemena.Domain.Enums;
using PodpiskaNaSemena.Domain.ValueObjects;
using PodpiskaNaSemena3.Domain.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PodpiskaNaSemena.Domain.Entities
{
    public class Subscription : Entity<int>
    {
        public ICollection<User> Users { get; private set; } = new List<User>();
        public int SeedId { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public SubscriptionStatus Status { get; private set; }
        public Payment? Payment { get; private set; }

        public Subscription(int seedId, DateTime startDate, DateTime endDate)
        {
            if (startDate >= endDate)
                throw new SubscriptionValidationException("Дата окончания должна быть позже даты начала.");

            SeedId = seedId;
            StartDate = startDate;
            EndDate = endDate;
            Status = SubscriptionStatus.Active;
        }

        // Методы для работы с пользователями
        public void AddUser(User user)
        {
            if (user == null)
                throw new DomainException("Пользователь не может быть null");

            if (Users.Contains(user))
                throw new DomainException("Пользователь уже добавлен к подписке");

            Users.Add(user);
        }

        public void RemoveUser(User user)
        {
            if (user == null)
                throw new DomainException("Пользователь не может быть null");

            Users.Remove(user);
        }

        // Проверки доступа
        public void CheckUserAccess(User user)
        {
            if (user == null)
                throw new DomainException("Пользователь не может быть null");

            if (!Users.Contains(user) && !user.IsAdmin)
                throw new SubscriptionAccessDeniedException(user.Id, Id);
        }

        public void CheckPayment()
        {
            if (Payment?.Status != Enums.PaymentStatus.Completed)
                throw new SubscriptionNotPaidException(Id);
        }

        // Методы создания подписок
        public static Subscription CreateMonthlySubscription(int seedId, DateTime startDate)
            => new Subscription(seedId, startDate, startDate.AddMonths(1));

        public static Subscription CreateYearlySubscription(int seedId, DateTime startDate)
            => new Subscription(seedId, startDate, startDate.AddYears(1));

        public static Subscription CreateQuarterlySubscription(int seedId, DateTime startDate)
            => new Subscription(seedId, startDate, startDate.AddMonths(3));

        // Расчет цены подписки
        public Amount CalculatePrice()
        {
            var duration = (EndDate - StartDate).Days;
            var amount = duration switch
            {
                <= 31 => 100m,    // Месяц
                <= 93 => 250m,    // Квартал
                _ => 800m         // Год
            };
            return new Amount(amount);
        }

        // Существующие методы
        public void Cancel()
        {
            if (Status != SubscriptionStatus.Active)
                throw new SubscriptionValidationException("Нельзя отменить неактивную подписку.");
            Status = SubscriptionStatus.Canceled;
        }

        public void LinkPayment(Payment payment)
        {
            Payment = payment ?? throw new DomainException("Платеж не может быть null.");
        }

        public bool IsActive(DateTime currentDate)
            => Status == SubscriptionStatus.Active && currentDate >= StartDate && currentDate <= EndDate;

        // Дополнительные методы
        public TimeSpan GetRemainingTime(DateTime currentDate)
            => IsActive(currentDate) ? EndDate - currentDate : TimeSpan.Zero;

        public bool IsExpiringSoon(DateTime currentDate, int daysThreshold = 7)
            => IsActive(currentDate) && (EndDate - currentDate).TotalDays <= daysThreshold;
    }
}