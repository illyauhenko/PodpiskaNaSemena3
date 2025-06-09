using PodpiskaNaSemena.Domain.ValueObjects.Base;
using PodpiskaNaSemena.Domain.ValueObjects.Exceptions;

public sealed class DescriptionValidator : IValidator<string>
{
    public void Validate(string value)
    {
        if (value.Length > 500)
            throw new ValidationException("Описание не должно превышать 500 символов");
    }
}