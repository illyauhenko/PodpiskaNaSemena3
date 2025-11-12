using PodpiskaNaSemena.Domain.ValueObjects.Base;
using PodpiskaNaSemena.Domain.ValueObjects.Validators;

namespace PodpiskaNaSemena.Domain.ValueObjects
{
    public sealed class DateRange : ValueObject<(DateTime Start, DateTime End)>
    {
        public DateTime Start => Value.Start;
        public DateTime End => Value.End;

        public DateRange(DateTime start, DateTime end)
            : base(new DateRangeValidator(), (start, end)) { }

        // Длительность диапазона
        public TimeSpan Duration => End - Start;

        // Проверка находится ли дата в диапазоне
        public bool Contains(DateTime date) => date >= Start && date <= End;

        // Проверка пересекается ли с другим диапазоном
        public bool Overlaps(DateRange other)
            => Start <= other.End && other.Start <= End;

        // Создание новых диапазонов
        public DateRange Extend(TimeSpan extension)
            => new(Start, End + extension);

        public DateRange Shift(TimeSpan shift)
            => new(Start + shift, End + shift);
    }
}