using PodpiskaNaSemena.Domain.ValueObjects.Base;
using PodpiskaNaSemena.Domain.ValueObjects.Exceptions;
using System.Linq;

namespace PodpiskaNaSemena.Domain.ValueObjects.Validators
{

    public sealed class PaymentMethodValidator : IValidator<string>
    {
        private static readonly string[] AllowedMethods = { "CreditCard", "PayPal" };

        public void Validate(string value)
        {
            if (!AllowedMethods.Contains(value))
                throw new ValidationException("Недопустимый метод оплаты");
        }
    }
}