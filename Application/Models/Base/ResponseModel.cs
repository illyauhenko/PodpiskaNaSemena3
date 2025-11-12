namespace PodpiskaNaSemena.Application.Models.Base
{
    /// <summary>
    /// Базовая модель для ответов API (DTO)
    /// Содержит идентификатор сущности
    /// </summary>
    public abstract record ResponseModel<TId>(TId Id) : IModel<TId> where TId : struct;
}