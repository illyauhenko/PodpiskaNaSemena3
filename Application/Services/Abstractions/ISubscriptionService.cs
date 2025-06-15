using PodpiskaNaSemena.Application.Models.Subscription;

public interface ISubscriptionService
{
    Task<SubscriptionModel> CreateAsync(SubscriptionCreateModel model, CancellationToken ct = default);
    Task CancelAsync(int subscriptionId, CancellationToken ct = default);
}