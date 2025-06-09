namespace PodpiskaNaSemena.Application.Models.Payment
{
    public sealed record PaymentCreateModel(
        int SubscriptionId,
        decimal Amount,
        string PaymentMethod
    );
}

