using PodpiskaNaSemena.Domain.ValueObjects.Base;
using PodpiskaNaSemena.Domain.ValueObjects.Validators;

namespace PodpiskaNaSemena.Domain.ValueObjects
{
    public sealed class Price : ValueObject<decimal>
    {
        public Price(decimal value)
            : base(new PriceValidator(), value) { }
    }
}