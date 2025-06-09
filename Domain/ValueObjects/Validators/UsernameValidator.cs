using PodpiskaNaSemena.Domain.ValueObjects.Base;
using PodpiskaNaSemena.Domain.ValueObjects.Exceptions;

namespace PodpiskaNaSemena.Domain.ValueObjects.Validators
{
    public sealed class UsernameValidator : IValidator<string>
    {
        public void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length < 3 || value.Length > 50)
                throw new ValidationException(ExceptionMessages.USERNAME_INVALID);
        }
    }
}