using PodpiskaNaSemena.Domain.Entities;
using PodpiskaNaSemena.Domain.Exceptions;
using PodpiskaNaSemena.Domain.ValueObjects;
using PodpiskaNaSemena3.Domain.Domain.Exceptions;
using System.Collections.Generic;

namespace PodpiskaNaSemena.Domain.Services
{
    public class SeedManagementService
    {
        // Создание семян (только для администраторов)
        public Seed CreateSeed(User creator, int id, SeedName name, Description description, Price price)
        {
            ValidateAdminAccess(creator);

            if (name == null)
                throw new DomainException("Название семени обязательно");

            if (description == null)
                throw new DomainException("Описание обязательно");

            if (price == null)
                throw new DomainException("Цена обязательна");

            return new Seed(id, name, description, price);
        }

        // Управление доступностью семян (только для администраторов)
        public void MakeSeedAvailable(User admin, Seed seed)
        {
            ValidateAdminAccess(admin);

            if (seed == null)
                throw new DomainException("Семена не могут быть null");

            seed.MarkAsAvailable();
        }

        public void MakeSeedUnavailable(User admin, Seed seed)
        {
            ValidateAdminAccess(admin);

            if (seed == null)
                throw new DomainException("Семена не могут быть null");

            seed.MarkAsUnavailable();
        }

        // Обновление цены семян (только для администраторов)
        public void UpdateSeedPrice(User admin, Seed seed, Price newPrice)
        {
            ValidateAdminAccess(admin);

            if (seed == null)
                throw new DomainException("Семена не могут быть null");

            seed.UpdatePrice(newPrice);
        }

        // Удаление семян (только для администраторов)
        public void DeleteSeed(User admin, Seed seed)
        {
            ValidateAdminAccess(admin);

            if (seed == null)
                throw new DomainException("Семена не могут быть null");

            // Проверяем нет ли активных подписок на эти семена
            if (seed.GetActiveSubscriptionsCount() > 0)
                throw new DomainException("Нельзя удалить семена с активными подписками");

            // В реальности здесь была бы пометка на удаление или вызов репозитория
            // seed.MarkAsDeleted(); если бы был такой метод
        }

        // Массовое добавление семян (только для администраторов)
        public List<Seed> CreateMultipleSeeds(User admin, params (int id, SeedName name, Description description, Price price)[] seedsData)
        {
            ValidateAdminAccess(admin);

            var seeds = new List<Seed>();
            foreach (var data in seedsData)
            {
                var seed = CreateSeed(admin, data.id, data.name, data.description, data.price);
                seeds.Add(seed);
            }

            return seeds;
        }

        // Вспомогательный метод проверки прав администратора
        private void ValidateAdminAccess(User user)
        {
            if (user == null)
                throw new DomainException("Пользователь не может быть null");

            if (!user.IsAdmin)
                throw new AdminRequiredException();
        }
    }
}