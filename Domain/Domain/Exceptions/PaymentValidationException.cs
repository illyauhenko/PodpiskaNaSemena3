using PodpiskaNaSemena.Domain.Exceptions;
namespace PodpiskaNaSemena.Domain.Exceptions
{
    public class PaymentValidationException : DomainException
    {
        public PaymentValidationException(string message) : base(message) { }

        // Дополнительные конструкторы для удобства
        public PaymentValidationException(int paymentId, string message)
            : base($"Платеж {paymentId}: {message}") { }

        public PaymentValidationException(string paymentType, string message)
            : base($"[{paymentType}]: {message}") { }
    }
}