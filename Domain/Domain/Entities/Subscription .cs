using PodpiskaNaSemena.Domain.Entities.Base;
using PodpiskaNaSemena.Domain.Exceptions;
using PodpiskaNaSemena.Domain.Enums;
namespace PodpiskaNaSemena.Domain.Entities
{
    public class Subscription : Entity<int>
    {
        public int UserId { get; }
        public int SeedId { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public SubscriptionStatus Status { get; private set; }
        public Payment? Payment { get; private set; }

        public Subscription(int userId, int seedId, DateTime startDate, DateTime endDate)
        {
            if (startDate >= endDate)
                throw new DomainException("Дата окончания должна быть позже даты начала.");

            UserId = userId;
            SeedId = seedId;
            StartDate = startDate;
            EndDate = endDate;
            Status = SubscriptionStatus.Active;
        }

        public void Cancel()
        {
            if (Status != SubscriptionStatus.Active)
                throw new DomainException("Нельзя отменить неактивную подписку.");

            Status = SubscriptionStatus.Canceled;
        }

        public void LinkPayment(Payment payment)
        {
            Payment = payment ?? throw new DomainException("Платеж не может быть null.");
        }

        public bool IsActive(DateTime currentDate)
            => Status == SubscriptionStatus.Active && currentDate >= StartDate && currentDate <= EndDate;
    }
}