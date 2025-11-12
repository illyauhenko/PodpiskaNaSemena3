namespace PodpiskaNaSemena.Application.Models.Payment
{
    /// <summary>
    /// Подробная информация о платеже (для админа или детальной страницы)
    /// Содержит дополнительную информацию через связи
    /// </summary>
    public sealed record PaymentDetailsResponse(
        int Id,
        int SubscriptionId,
        string UserName,        
        string SeedName,        
        decimal Amount,
        string PaymentMethod,
        DateTime PaymentDate,
        string Status,
        string? FailureReason   // Причина отказа если платеж failed
    );
}