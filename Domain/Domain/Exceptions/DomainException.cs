namespace PodpiskaNaSemena.Domain.Exceptions
{
    /// <summary>
    /// Базовое исключение для доменных ошибок
    /// </summary>
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message) { }
    }
}