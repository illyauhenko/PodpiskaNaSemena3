
using PodpiskaNaSemena.Domain.ValueObjects.Base;
using PodpiskaNaSemena.Domain.ValueObjects.Exceptions;

namespace PodpiskaNaSemena.Domain.ValueObjects.Validators
{
    public sealed class SeedNameValidator : IValidator<string>
    {
        public void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > 100)
                throw new ValidationException("Название семени должно быть от 1 до 100 символов");
        }
    }
}
