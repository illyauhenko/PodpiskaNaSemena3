using PodpiskaNaSemena.Domain.ValueObjects.Base;
using PodpiskaNaSemena.Domain.ValueObjects.Validators;

namespace PodpiskaNaSemena.Domain.ValueObjects
{
    public sealed class Amount : ValueObject<decimal>
    {
        public Amount(decimal value)
            : base(new AmountValidator(), value) { }
    }
}