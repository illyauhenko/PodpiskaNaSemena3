namespace PodpiskaNaSemena.Application.Models.Base
{
    /// <summary>
    /// Базовая модель для запросов обновления
    /// Содержит идентификатор сущности для обновления
    /// </summary>
    public abstract record UpdateRequestModel<TId>(TId Id) : IModel<TId> where TId : struct;
}