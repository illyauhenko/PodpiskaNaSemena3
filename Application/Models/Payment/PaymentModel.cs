
using PodpiskaNaSemena.Application.Models.Base;

namespace PodpiskaNaSemena.Application.Models.Payment
{
    public sealed record PaymentModel(
        int Id,
        string Username,
        decimal Amount,
        string PaymentMethod,
        DateTime PaymentDate,
        string Status
    ) : Model(Id, Username);
}