using PodpiskaNaSemena.Application.Models.Base;

namespace PodpiskaNaSemena.Application.Models.Subscription
{
    public sealed record SubscriptionModel(
        int Id,
        string Username,
        string SeedName,
        DateTime StartDate,
        DateTime EndDate,
        string Status
    ) : Model(Id, Username);
}