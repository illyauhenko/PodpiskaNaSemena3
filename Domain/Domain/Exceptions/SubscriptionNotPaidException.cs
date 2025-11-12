namespace PodpiskaNaSemena.Domain.Exceptions
{
    public class SubscriptionNotPaidException : DomainException
    {
        public SubscriptionNotPaidException(int subscriptionId)
            : base($"Подписка {subscriptionId} не оплачена")
        { }
    }
}