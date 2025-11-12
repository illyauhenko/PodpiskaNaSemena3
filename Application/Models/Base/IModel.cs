namespace PodpiskaNaSemena.Application.Models.Base
{
    /// <summary>
    /// Базовый интерфейс для всех моделей с идентификатором
    /// </summary>
    public interface IModel<TId> where TId : struct
    {
        TId Id { get; }
    }
}