using PodpiskaNaSemena.Domain.Entities;
using PodpiskaNaSemena.Domain.Exceptions;
using PodpiskaNaSemena.Domain.ValueObjects;
using PodpiskaNaSemena.Domain.Enums;
using PodpiskaNaSemena3.Domain.Domain.Exceptions;

namespace PodpiskaNaSemena.Domain.Services
{
    public class UserManagementService
    {
        // Создание обычного пользователя (может любой)
        public User CreateRegularUser(int id, Username username, Email email)
        {
            return new User(id, username, email);
        }

        // Создание администратора (только для существующих админов)
        public User CreateAdminUser(User creator, int id, Username username, Email email)
        {
            if (creator == null)
                throw new DomainException("Создатель не может быть null");

            if (!creator.IsAdmin)
                throw new AdminRequiredException();

            var adminUser = new User(id, username, email);
            adminUser.MakeAdmin(); // Используем существующий метод

            return adminUser;
        }

        // Создание подписки (может любой пользователь)
        public Subscription CreateSubscription(User creator, int seedId, SubscriptionType type)
        {
            if (creator == null)
                throw new DomainException("Создатель не может быть null");

            var startDate = DateTime.UtcNow;
            DateTime endDate = type switch
            {
                SubscriptionType.Monthly => startDate.AddMonths(1),
                SubscriptionType.Quarterly => startDate.AddMonths(3),
                SubscriptionType.Yearly => startDate.AddYears(1),
                _ => throw new DomainException("Неизвестный тип подписки")
            };

            var subscription = new Subscription(seedId, startDate, endDate);
            subscription.AddUser(creator); // Автоматически добавляем создателя

            return subscription;
        }
    }
}