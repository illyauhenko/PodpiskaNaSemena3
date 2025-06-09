namespace PodpiskaNaSemena.Domain.Exceptions
{
    /// <summary>
    /// Исключение при отсутствии сущности
    /// </summary>
    public sealed class EntityNotFoundException : DomainException
    {
        public EntityNotFoundException(string entityName, object id)
            : base($"Сущность '{entityName}' с ID {id} не найдена") { }
    }
}