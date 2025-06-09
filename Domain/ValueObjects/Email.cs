using PodpiskaNaSemena.Domain.ValueObjects.Base;
using PodpiskaNaSemena.Domain.ValueObjects.Validators;

namespace PodpiskaNaSemena.Domain.ValueObjects
{
    public sealed class Email : ValueObject<string>
    {
        public Email(string value)
            : base(new EmailValidator(), value.Trim()) { }
    }
}