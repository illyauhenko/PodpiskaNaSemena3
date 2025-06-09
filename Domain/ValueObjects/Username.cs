using PodpiskaNaSemena.Domain.ValueObjects.Base;
using PodpiskaNaSemena.Domain.ValueObjects.Validators;

namespace PodpiskaNaSemena.Domain.ValueObjects
{
    public sealed class Username : ValueObject<string>
    {
        public Username(string value)
            : base(new UsernameValidator(), value.Trim()) { }
    }
}