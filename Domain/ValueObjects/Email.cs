using PodpiskaNaSemena.Domain.ValueObjects.Base;
using PodpiskaNaSemena.Domain.ValueObjects.Validators;
using System.Linq;

namespace PodpiskaNaSemena.Domain.ValueObjects
{
    public sealed class Email : ValueObject<string>
    {
        public Email(string value)
            : base(new EmailValidator(), value.Trim()) { }

        // Получение домена email
        public string GetDomain()
        {
            var parts = Value.Split('@');
            return parts.Length == 2 ? parts[1] : string.Empty;
        }

        // Получение имени пользователя из email
        public string GetUsernamePart()
        {
            var parts = Value.Split('@');
            return parts.Length == 2 ? parts[0] : string.Empty;
        }

        // Проверка является ли email корпоративным (опционально)
        public bool IsCorporateEmail()
        {
            var domain = GetDomain();
            var corporateDomains = new[] { "company.com", "business.org" };
            return corporateDomains.Contains(domain);
        }
    }
}