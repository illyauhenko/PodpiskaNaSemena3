using PodpiskaNaSemena.Application.Models.Payment;

public interface IPaymentService
{
    Task<PaymentModel> ProcessPaymentAsync(PaymentCreateModel model, CancellationToken ct = default);
}