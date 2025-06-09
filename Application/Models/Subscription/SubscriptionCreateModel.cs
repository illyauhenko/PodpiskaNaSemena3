namespace PodpiskaNaSemena.Application.Models.Subscription
{
    public sealed record SubscriptionCreateModel(
        int UserId,
        int SeedId,
        DateTime StartDate,
        DateTime EndDate
    );
}