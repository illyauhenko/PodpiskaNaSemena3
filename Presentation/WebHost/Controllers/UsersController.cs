using Microsoft.AspNetCore.Mvc;
using PodpiskaNaSemena.Application.Models.User;
using PodpiskaNaSemena.Application.Services;
using PodpiskaNaSemena.Application.Services.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace PodpiskaNaSemena.Presentation.WebHost.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserResponse>> CreateUser([FromBody] CreateUserRequest request)
        {
            _logger.LogInformation("Creating new user with username: {Username}", request.Username);

            try
            {
                var user = await _userService.CreateUserAsync(request);
                _logger.LogInformation("User created successfully with ID: {UserId}", user.Id);

                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user with username: {Username}", request.Username);
                throw;
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponse>> GetUser([Range(1, int.MaxValue)] int id)
        {
            _logger.LogInformation("Getting user with ID: {UserId}", id);

            var user = await _userService.GetUserAsync(id);
            return Ok(user);
        }

        [HttpGet("{id:int}/details")]
        [ProducesResponseType(typeof(UserDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDetailsResponse>> GetUserDetails([Range(1, int.MaxValue)] int id)
        {
            _logger.LogInformation("Getting user details for ID: {UserId}", id);

            var userDetails = await _userService.GetUserDetailsAsync(id);
            return Ok(userDetails);
        }

        [HttpPost("{id:int}/make-admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> MakeUserAdmin([Range(1, int.MaxValue)] int id)
        {
            _logger.LogInformation("Making user with ID: {UserId} an admin", id);

            await _userService.MakeUserAdminAsync(id);
            return NoContent();
        }

        [HttpPost("{id:int}/remove-admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveUserAdmin([Range(1, int.MaxValue)] int id)
        {
            _logger.LogInformation("Removing admin rights from user with ID: {UserId}", id);

            await _userService.RemoveUserAdminAsync(id);
            return NoContent();
        }

        [HttpGet("check-email")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> IsEmailUnique([FromQuery][EmailAddress] string email)
        {
            _logger.LogInformation("Checking email uniqueness: {Email}", email);

            var isUnique = await _userService.IsEmailUniqueAsync(email);
            return Ok(isUnique);
        }
    }
}