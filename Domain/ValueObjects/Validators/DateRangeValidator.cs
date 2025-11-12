using PodpiskaNaSemena.Domain.ValueObjects.Base;
using PodpiskaNaSemena.Domain.ValueObjects.Exceptions;

namespace PodpiskaNaSemena.Domain.ValueObjects.Validators
{
    public sealed class DateRangeValidator : IValidator<(DateTime Start, DateTime End)>
    {
        public void Validate((DateTime Start, DateTime End) value)
        {
            if (value.Start >= value.End)
                throw new ValidationException("Дата окончания должна быть позже даты начала");

            if (value.Start < DateTime.UtcNow.AddYears(-1))
                throw new ValidationException("Дата начала не может быть больше года назад");

            if (value.End > DateTime.UtcNow.AddYears(10))
                throw new ValidationException("Дата окончания не может быть больше 10 лет вперед");
        }
    }
}