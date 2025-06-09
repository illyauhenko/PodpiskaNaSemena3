using PodpiskaNaSemena.Domain.ValueObjects.Base;
using PodpiskaNaSemena.Domain.ValueObjects.Exceptions;

public sealed class AmountValidator : IValidator<decimal>
{
    public void Validate(decimal value)
    {
        if (value <= 0)
            throw new ValidationException("Сумма платежа должна быть положительной");
    }
}