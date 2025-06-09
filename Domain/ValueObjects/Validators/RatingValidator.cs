using PodpiskaNaSemena.Domain.ValueObjects.Base;
using PodpiskaNaSemena.Domain.ValueObjects.Exceptions;

public sealed class RatingValidator : IValidator<int>
{
    public void Validate(int value)
    {
        if (value < 1 || value > 5)
            throw new ValidationException("Рейтинг должен быть от 1 до 5");
    }
}