using Microsoft.AspNetCore.Mvc;
using PodpiskaNaSemena.Application.Models.Subscription;
using PodpiskaNaSemena.Application.Services;
using PodpiskaNaSemena.Application.Services.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace PodpiskaNaSemena.Presentation.WebHost.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly ILogger<SubscriptionsController> _logger;

        public SubscriptionsController(ISubscriptionService subscriptionService, ILogger<SubscriptionsController> logger)
        {
            _subscriptionService = subscriptionService;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SubscriptionResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SubscriptionResponse>> CreateSubscription([FromBody] CreateSubscriptionRequest request)
        {
            _logger.LogInformation("Creating subscription for user {UserId} on seed {SeedId}",
                request.UserId, request.SeedId);

            var subscription = await _subscriptionService.CreateSubscriptionAsync(request);
            _logger.LogInformation("Subscription created successfully with ID: {SubscriptionId}", subscription.Id);

            return CreatedAtAction(nameof(GetSubscription), new { id = subscription.Id }, subscription);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(SubscriptionDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SubscriptionDetailsResponse>> GetSubscription([Range(1, int.MaxValue)] int id)
        {
            _logger.LogInformation("Getting subscription with ID: {SubscriptionId}", id);

            var subscription = await _subscriptionService.GetSubscriptionAsync(id);
            return Ok(subscription);
        }

        [HttpGet("user/{userId:int}")]
        [ProducesResponseType(typeof(IReadOnlyList<SubscriptionResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<SubscriptionResponse>>> GetUserSubscriptions([Range(1, int.MaxValue)] int userId)
        {
            _logger.LogInformation("Getting subscriptions for user ID: {UserId}", userId);

            var subscriptions = await _subscriptionService.GetUserSubscriptionsAsync(userId);
            return Ok(subscriptions);
        }

        [HttpGet("active")]
        [ProducesResponseType(typeof(IReadOnlyList<SubscriptionResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<SubscriptionResponse>>> GetActiveSubscriptions()
        {
            _logger.LogInformation("Getting all active subscriptions");

            var subscriptions = await _subscriptionService.GetActiveSubscriptionsAsync();
            return Ok(subscriptions);
        }

        [HttpPost("{id:int}/cancel")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelSubscription([Range(1, int.MaxValue)] int id)
        {
            _logger.LogInformation("Cancelling subscription with ID: {SubscriptionId}", id);

            await _subscriptionService.CancelSubscriptionAsync(id);
            return NoContent();
        }

        [HttpGet("check-active")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> HasActiveSubscription(
            [FromQuery][Range(1, int.MaxValue)] int userId,
            [FromQuery][Range(1, int.MaxValue)] int seedId)
        {
            _logger.LogInformation("Checking active subscription for user {UserId} on seed {SeedId}", userId, seedId);

            var hasActiveSubscription = await _subscriptionService.HasActiveSubscriptionAsync(userId, seedId);
            return Ok(hasActiveSubscription);
        }
    }
}