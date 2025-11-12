using PodpiskaNaSemena.Domain.ValueObjects.Base;
using PodpiskaNaSemena.Domain.ValueObjects.Validators;
using System.Collections.Generic;

namespace PodpiskaNaSemena.Domain.ValueObjects
{
    public sealed class PaymentMethod : ValueObject<string>
    {
        public PaymentMethod(string value)
            : base(new PaymentMethodValidator(), value.Trim()) { }

        // Предопределенные методы оплаты
        public static PaymentMethod CreditCard => new("CreditCard");
        public static PaymentMethod PayPal => new("PayPal");
        public static PaymentMethod BankTransfer => new("BankTransfer");
        public static PaymentMethod Crypto => new("Crypto");

        // Проверка типа метода оплаты
        public bool IsCardPayment => Value == "CreditCard";
        public bool IsDigitalWallet => Value == "PayPal";
        public bool IsBankPayment => Value == "BankTransfer";
        public bool IsCrypto => Value == "Crypto";

        // Получение всех доступных методов
        public static IEnumerable<PaymentMethod> GetAllMethods()
        {
            return new[] { CreditCard, PayPal, BankTransfer, Crypto };
        }
    }
}