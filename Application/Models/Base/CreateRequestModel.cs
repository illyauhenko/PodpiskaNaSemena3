namespace PodpiskaNaSemena.Application.Models.Base
{
    /// <summary>
    /// Базовая модель для запросов создания
    /// НЕ содержит идентификатор - он генерируется на сервере
    /// </summary>
    public abstract record CreateRequestModel;
}