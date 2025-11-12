using PodpiskaNaSemena.Domain.ValueObjects.Base;
using PodpiskaNaSemena.Domain.ValueObjects.Exceptions;

namespace PodpiskaNaSemena.Domain.ValueObjects.Validators
{

    public sealed class CommentValidator : IValidator<string>
    {
        public void Validate(string value)
        {
            if (value.Length > 1000)
                throw new ValidationException("Комментарий не должен превышать 1000 символов");
        }
    }
}