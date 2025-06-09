using PodpiskaNaSemena.Domain.ValueObjects.Base;
using PodpiskaNaSemena.Domain.ValueObjects.Validators;

namespace PodpiskaNaSemena.Domain.ValueObjects
{
    public sealed class Description : ValueObject<string>
    {
        public Description(string value)
            : base(new DescriptionValidator(), value.Trim()) { }
    }
}