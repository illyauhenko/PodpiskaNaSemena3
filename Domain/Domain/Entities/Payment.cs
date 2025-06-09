using PodpiskaNaSemena.Domain.Entities.Base;
using PodpiskaNaSemena.Domain.Exceptions;
using PodpiskaNaSemena.Domain.Enums;

namespace PodpiskaNaSemena.Domain.Entities
{
    public class Payment : Entity<int>
    {
        public int SubscriptionId { get; }
        public decimal Amount { get; }
        public string PaymentMethod { get; }
        public DateTime PaymentDate { get; }
        public PaymentStatus Status { get; private set; }

        public Payment(int subscriptionId, decimal amount, string paymentMethod)
        {
            if (amount <= 0)
                throw new DomainException("Сумма платежа должна быть положительной.");

            SubscriptionId = subscriptionId;
            Amount = amount;
            PaymentMethod = paymentMethod;
            PaymentDate = DateTime.UtcNow;
            Status = PaymentStatus.Pending;
        }

        public void MarkAsPaid() => Status = PaymentStatus.Completed;
        public void MarkAsFailed() => Status = PaymentStatus.Failed;
    }
}