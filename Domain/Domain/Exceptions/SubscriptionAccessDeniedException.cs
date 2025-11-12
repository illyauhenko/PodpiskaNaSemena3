using PodpiskaNaSemena.Domain.Exceptions;

namespace PodpiskaNaSemena3.Domain.Domain.Exceptions
{
    public class SubscriptionAccessDeniedException : DomainException
    {
        public SubscriptionAccessDeniedException(int userId, int subscriptionId)
            : base($"Пользователь {userId} не имеет доступа к подписке {subscriptionId}")
        { }
    }
}
