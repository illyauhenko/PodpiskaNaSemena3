namespace PodpiskaNaSemena.Domain.ValueObjects.Exceptions
{
    public class ValidatorNullException : Exception
    {
        public ValidatorNullException(string className, string message)
            : base($"[{className}]: {message}") { }
    }
}