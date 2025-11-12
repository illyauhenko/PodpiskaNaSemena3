using Microsoft.AspNetCore.Mvc;
using PodpiskaNaSemena.Application.Models.Review;
using PodpiskaNaSemena.Application.Services;
using PodpiskaNaSemena.Application.Services.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace PodpiskaNaSemena.Presentation.WebHost.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly ILogger<ReviewsController> _logger;

        public ReviewsController(IReviewService reviewService, ILogger<ReviewsController> logger)
        {
            _reviewService = reviewService;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ReviewResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ReviewResponse>> CreateReview([FromBody] CreateReviewRequest request)
        {
            _logger.LogInformation("Creating review by user {UserId} for seed {SeedId}",
                request.UserId, request.SeedId);

            var review = await _reviewService.CreateReviewAsync(request);
            _logger.LogInformation("Review created successfully with ID: {ReviewId}", review.Id);

            return CreatedAtAction(nameof(GetReview), new { id = review.Id }, review);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ReviewResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReviewResponse>> GetReview([Range(1, int.MaxValue)] int id)
        {
            _logger.LogInformation("Getting review with ID: {ReviewId}", id);

            var review = await _reviewService.GetReviewAsync(id);
            return Ok(review);
        }

        [HttpGet("seed/{seedId:int}")]
        [ProducesResponseType(typeof(IReadOnlyList<ReviewResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<ReviewResponse>>> GetReviewsForSeed([Range(1, int.MaxValue)] int seedId)
        {
            _logger.LogInformation("Getting reviews for seed ID: {SeedId}", seedId);

            var reviews = await _reviewService.GetReviewsForSeedAsync(seedId);
            return Ok(reviews);
        }

        [HttpGet("user/{userId:int}")]
        [ProducesResponseType(typeof(IReadOnlyList<ReviewResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<ReviewResponse>>> GetUserReviews([Range(1, int.MaxValue)] int userId)
        {
            _logger.LogInformation("Getting reviews by user ID: {UserId}", userId);

            var reviews = await _reviewService.GetUserReviewsAsync(userId);
            return Ok(reviews);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ReviewResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ReviewResponse>> UpdateReview(
            [Range(1, int.MaxValue)] int id,
            [FromBody] CreateReviewRequest request)
        {
            _logger.LogInformation("Updating review with ID: {ReviewId}", id);

            var updatedReview = await _reviewService.UpdateReviewAsync(id, request);
            return Ok(updatedReview);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteReview([Range(1, int.MaxValue)] int id)
        {
            _logger.LogInformation("Deleting review with ID: {ReviewId}", id);

            await _reviewService.DeleteReviewAsync(id);
            return NoContent();
        }

        [HttpGet("seed/{seedId:int}/average-rating")]
        [ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
        public async Task<ActionResult<double>> GetAverageRating([Range(1, int.MaxValue)] int seedId)
        {
            _logger.LogInformation("Getting average rating for seed ID: {SeedId}", seedId);

            var averageRating = await _reviewService.GetAverageRatingAsync(seedId);
            return Ok(averageRating);
        }

        [HttpGet("check-reviewed")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> HasUserReviewedSeed(
            [FromQuery][Range(1, int.MaxValue)] int userId,
            [FromQuery][Range(1, int.MaxValue)] int seedId)
        {
            _logger.LogInformation("Checking if user {UserId} reviewed seed {SeedId}", userId, seedId);

            var hasReviewed = await _reviewService.HasUserReviewedSeedAsync(userId, seedId);
            return Ok(hasReviewed);
        }
    }
}