using Microsoft.AspNetCore.Mvc;
using PodpiskaNaSemena.Application.Models.Seed;
using PodpiskaNaSemena.Application.Services;
using PodpiskaNaSemena.Application.Services.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace PodpiskaNaSemena.Presentation.WebHost.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class SeedsController : ControllerBase
    {
        private readonly ISeedService _seedService;
        private readonly ILogger<SeedsController> _logger;

        public SeedsController(ISeedService seedService, ILogger<SeedsController> logger)
        {
            _seedService = seedService;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SeedResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SeedResponse>> CreateSeed([FromBody] CreateSeedRequest request)
        {
            _logger.LogInformation("Creating new seed with name: {SeedName}", request.Name);

            var seed = await _seedService.CreateSeedAsync(request);
            _logger.LogInformation("Seed created successfully with ID: {SeedId}", seed.Id);

            return CreatedAtAction(nameof(GetSeed), new { id = seed.Id }, seed);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(SeedResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SeedResponse>> GetSeed([Range(1, int.MaxValue)] int id)
        {
            _logger.LogInformation("Getting seed with ID: {SeedId}", id);

            var seed = await _seedService.GetSeedAsync(id);
            return Ok(seed);
        }

        [HttpGet("{id:int}/details")]
        [ProducesResponseType(typeof(SeedDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SeedDetailsResponse>> GetSeedDetails([Range(1, int.MaxValue)] int id)
        {
            _logger.LogInformation("Getting seed details for ID: {SeedId}", id);

            var seedDetails = await _seedService.GetSeedDetailsAsync(id);
            return Ok(seedDetails);
        }

        [HttpGet("{id:int}/with-reviews")]
        [ProducesResponseType(typeof(SeedWithReviewsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SeedWithReviewsResponse>> GetSeedWithReviews([Range(1, int.MaxValue)] int id)
        {
            _logger.LogInformation("Getting seed with reviews for ID: {SeedId}", id);

            var seedWithReviews = await _seedService.GetSeedWithReviewsAsync(id);
            return Ok(seedWithReviews);
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(IReadOnlyList<SeedResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<SeedResponse>>> SearchSeeds([FromQuery] string keyword)
        {
            _logger.LogInformation("Searching seeds with keyword: {Keyword}", keyword);

            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest("Keyword is required");

            var seeds = await _seedService.SearchSeedsAsync(keyword);
            return Ok(seeds);
        }

        [HttpGet("top-rated")]
        [ProducesResponseType(typeof(IReadOnlyList<SeedResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<SeedResponse>>> GetTopRatedSeeds([FromQuery][Range(1, 50)] int count = 10)
        {
            _logger.LogInformation("Getting top {Count} rated seeds", count);

            var seeds = await _seedService.GetTopRatedSeedsAsync(count);
            return Ok(seeds);
        }

        [HttpGet("available")]
        [ProducesResponseType(typeof(IReadOnlyList<SeedResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<SeedResponse>>> GetAvailableSeeds()
        {
            _logger.LogInformation("Getting all available seeds");

            var seeds = await _seedService.GetAvailableSeedsAsync();
            return Ok(seeds);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(SeedResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SeedResponse>> UpdateSeed([Range(1, int.MaxValue)] int id, [FromBody] CreateSeedRequest request)
        {
            _logger.LogInformation("Updating seed with ID: {SeedId}", id);

            var updatedSeed = await _seedService.UpdateSeedAsync(id, request);
            return Ok(updatedSeed);
        }
    }
}