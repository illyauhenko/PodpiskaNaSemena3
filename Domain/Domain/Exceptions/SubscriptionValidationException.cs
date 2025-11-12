using PodpiskaNaSemena.Domain.Exceptions;

namespace PodpiskaNaSemena.Domain.Exceptions
{
    public class SubscriptionValidationException : DomainException
    {
        public SubscriptionValidationException(string message) : base(message) { }

        // Дополнительные конструкторы для удобства
        public SubscriptionValidationException(int subscriptionId, string message)
            : base($"Подписка {subscriptionId}: {message}") { }

        public SubscriptionValidationException(string subscriptionType, string message)
            : base($"[{subscriptionType}]: {message}") { }

        public SubscriptionValidationException(int subscriptionId, string operation, string message)
            : base($"Подписка {subscriptionId}, операция '{operation}': {message}") { }
    }
}