using PodpiskaNaSemena.Application.Models.Review;

public interface IReviewService
{
    Task<ReviewModel> CreateAsync(ReviewCreateModel model, CancellationToken ct = default);
}