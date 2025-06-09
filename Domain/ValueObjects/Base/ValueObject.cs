using PodpiskaNaSemena.Domain.ValueObjects.Exceptions;

namespace PodpiskaNaSemena.Domain.ValueObjects.Base
{
    public abstract class ValueObject<T> : IEquatable<ValueObject<T>>
    {
        public T Value { get; }

        protected ValueObject(IValidator<T> validator, T value)
        {
            if (validator == null)
                throw new ValidatorNullException(
                    GetType().FullName ?? string.Empty,
                    ExceptionMessages.VALIDATOR_MUST_BE_SPECIFIED
                );

            validator.Validate(value);
            Value = value;
        }

        public override string ToString() => Value?.ToString() ?? GetType().ToString();
        public override int GetHashCode() => Value?.GetHashCode() ?? 0;

        public override bool Equals(object? other)
            => Equals(other as ValueObject<T>);

        public bool Equals(ValueObject<T>? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return GetType() == other.GetType() &&
                   EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public static bool operator ==(ValueObject<T>? left, ValueObject<T>? right)
            => Equals(left, right);

        public static bool operator !=(ValueObject<T>? left, ValueObject<T>? right)
            => !(left == right);
    }
}