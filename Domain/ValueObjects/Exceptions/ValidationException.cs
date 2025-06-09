namespace PodpiskaNaSemena.Domain.ValueObjects.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }
}