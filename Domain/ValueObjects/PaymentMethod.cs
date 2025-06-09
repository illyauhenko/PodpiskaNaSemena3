using PodpiskaNaSemena.Domain.ValueObjects.Base;
using PodpiskaNaSemena.Domain.ValueObjects.Validators;

namespace PodpiskaNaSemena.Domain.ValueObjects
{
    public sealed class PaymentMethod : ValueObject<string>
    {
        public PaymentMethod(string value)
            : base(new PaymentMethodValidator(), value.Trim()) { }
    }
}