using PodpiskaNaSemena.Application.Models.Review;

namespace PodpiskaNaSemena.Application.Models.Seed
{
    /// <summary>
    /// Семена с отзывами (для страницы товара)
    /// </summary>
    public sealed record SeedWithReviewsResponse(
        int Id,
        string Name,
        string Description,
        decimal Price,
        double AverageRating,
        List<ReviewResponse> Reviews // Вложенные отзывы
    );
}