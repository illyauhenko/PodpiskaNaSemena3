using PodpiskaNaSemena.Application.Models.Base;

namespace PodpiskaNaSemena.Application.Models.Seed
{
    /// <summary>
    /// Запрос на создание семян (только для администраторов)
    /// </summary>
    public sealed record CreateSeedRequest(
        string Name,        // Название семян
        string Description, // Описание
        decimal Price       // Цена
    ) : CreateRequestModel;
}