using PodpiskaNaSemena.Domain.Entities.Base;
using PodpiskaNaSemena.Domain.Exceptions;
using PodpiskaNaSemena.Domain.Enums;
using PodpiskaNaSemena.Domain.ValueObjects;

namespace PodpiskaNaSemena.Domain.Entities
{
    public class Payment : Entity<int>
    {
        public int SubscriptionId { get; }
        public Amount Amount { get; }
        public PaymentMethod PaymentMethod { get; }
        public DateTime PaymentDate { get; }
        public PaymentStatus Status { get; private set; }
        public string? FailureReason { get; private set; }

        public Payment(int subscriptionId, Amount amount, PaymentMethod paymentMethod)
        {
            if (amount == null)
                throw new PaymentValidationException("Сумма платежа обязательна");

            if (paymentMethod == null)
                throw new PaymentValidationException("Метод оплаты обязателен");

            SubscriptionId = subscriptionId;
            Amount = amount;
            PaymentMethod = paymentMethod;
            PaymentDate = DateTime.UtcNow;
            Status = PaymentStatus.Pending;
        }

        // Основные методы изменения статуса
        public void MarkAsPaid()
        {
            if (Status != PaymentStatus.Pending)
                throw new PaymentValidationException("Можно подтвердить только ожидающий платеж.");
            Status = PaymentStatus.Completed;
        }

        public void MarkAsFailed(string reason)
        {
            if (Status != PaymentStatus.Pending)
                throw new PaymentValidationException("Можно отклонить только ожидающий платеж.");

            if (string.IsNullOrWhiteSpace(reason))
                throw new PaymentValidationException("Причина отказа обязательна.");

            Status = PaymentStatus.Failed;
            FailureReason = reason;
        }

        public void Refund(string reason)
        {
            if (Status != PaymentStatus.Completed)
                throw new PaymentValidationException("Можно вернуть только завершенный платеж.");

            if (string.IsNullOrWhiteSpace(reason))
                throw new PaymentValidationException("Причина возврата обязательна.");

            Status = PaymentStatus.Refunded;
            FailureReason = reason;
        }

        // Проверочные методы
        public bool IsSuccessful => Status == PaymentStatus.Completed;
        public bool CanBeProcessed => Status == PaymentStatus.Pending;

        // Валидация платежа для подписки
        public void ValidateForSubscription(Subscription subscription)
        {
            if (subscription == null)
                throw new DomainException("Подписка не может быть null");

            var subscriptionPrice = subscription.CalculatePrice();
            if (Amount.Value != subscriptionPrice.Value)
                throw new PaymentValidationException("Сумма платежа не соответствует стоимости подписки.");
        }

        // Статический метод создания платежа
        public static Payment CreateForSubscription(Subscription subscription, PaymentMethod paymentMethod)
        {
            if (subscription == null)
                throw new DomainException("Подписка не может быть null");

            var amount = subscription.CalculatePrice();
            return new Payment(subscription.Id, amount, paymentMethod);
        }

        // Дополнительные методы
        public bool IsCardPayment => PaymentMethod.IsCardPayment;
        public bool IsDigitalWallet => PaymentMethod.IsDigitalWallet;
        public string GetAmountFormatted() => Amount.Value.ToString("C");
    }
}