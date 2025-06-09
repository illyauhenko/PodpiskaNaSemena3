using PodpiskaNaSemena.Domain.ValueObjects.Base;
using PodpiskaNaSemena.Domain.ValueObjects.Validators;

namespace PodpiskaNaSemena.Domain.ValueObjects
{
    public sealed class SeedName : ValueObject<string>
    {
        public SeedName(string value)
            : base(new SeedNameValidator(), value.Trim()) { }
    }
}