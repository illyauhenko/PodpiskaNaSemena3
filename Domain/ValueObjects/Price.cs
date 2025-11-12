using PodpiskaNaSemena.Domain.ValueObjects.Base;
using PodpiskaNaSemena.Domain.ValueObjects.Validators;

namespace PodpiskaNaSemena.Domain.ValueObjects
{
    public sealed class Price : ValueObject<decimal>
    {
        public Price(decimal value)
            : base(new PriceValidator(), value) { }

        // Математические операции с ценами
        public Price Add(Price other) => new(Value + other.Value);
        public Price Subtract(Price other) => new(Value - other.Value);
        public Price Multiply(decimal multiplier) => new(Value * multiplier);
        public Price ApplyDiscount(decimal discountPercent)
            => new(Value * (1 - discountPercent / 100));

        // Сравнение цен
        public bool IsGreaterThan(Price other) => Value > other.Value;
        public bool IsLessThan(Price other) => Value < other.Value;
        public bool IsEqualTo(Price other) => Value == other.Value;

        // Форматирование цены
        public string ToString(string format) => Value.ToString(format);
        public string ToCurrencyString() => $"{Value:C}";
    }
}