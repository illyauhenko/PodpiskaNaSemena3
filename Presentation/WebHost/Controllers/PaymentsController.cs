using Microsoft.AspNetCore.Mvc;
using PodpiskaNaSemena.Application.Models.Payment;
using PodpiskaNaSemena.Application.Services;
using PodpiskaNaSemena.Application.Services.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace PodpiskaNaSemena.Presentation.WebHost.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(PaymentResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaymentResponse>> CreatePayment([FromBody] CreatePaymentRequest request)
        {
            _logger.LogInformation("Creating payment for subscription {SubscriptionId}", request.SubscriptionId);

            var payment = await _paymentService.CreatePaymentAsync(request);
            _logger.LogInformation("Payment created successfully with ID: {PaymentId}", payment.Id);

            return CreatedAtAction(nameof(GetPayment), new { id = payment.Id }, payment);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(PaymentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaymentResponse>> GetPayment([Range(1, int.MaxValue)] int id)
        {
            _logger.LogInformation("Getting payment with ID: {PaymentId}", id);

            var payment = await _paymentService.GetPaymentAsync(id);
            return Ok(payment);
        }

        [HttpGet("subscription/{subscriptionId:int}")]
        [ProducesResponseType(typeof(PaymentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<PaymentResponse>> GetPaymentBySubscription([Range(1, int.MaxValue)] int subscriptionId)
        {
            _logger.LogInformation("Getting payment for subscription ID: {SubscriptionId}", subscriptionId);

            var payment = await _paymentService.GetPaymentBySubscriptionAsync(subscriptionId);
            return payment != null ? Ok(payment) : NoContent();
        }

        [HttpPost("{id:int}/process")]
        [ProducesResponseType(typeof(PaymentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaymentResponse>> ProcessPayment([Range(1, int.MaxValue)] int id)
        {
            _logger.LogInformation("Processing payment with ID: {PaymentId}", id);

            var payment = await _paymentService.ProcessPaymentAsync(id);
            return Ok(payment);
        }

        [HttpPost("{id:int}/mark-paid")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> MarkPaymentAsPaid([Range(1, int.MaxValue)] int id)
        {
            _logger.LogInformation("Marking payment {PaymentId} as paid", id);

            await _paymentService.MarkPaymentAsPaidAsync(id);
            return NoContent();
        }

        [HttpPost("{id:int}/mark-failed")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> MarkPaymentAsFailed(
            [Range(1, int.MaxValue)] int id,
            [FromBody] string reason)
        {
            _logger.LogInformation("Marking payment {PaymentId} as failed. Reason: {Reason}", id, reason);

            await _paymentService.MarkPaymentAsFailedAsync(id, reason);
            return NoContent();
        }

        [HttpGet("pending")]
        [ProducesResponseType(typeof(IReadOnlyList<PaymentResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<PaymentResponse>>> GetPendingPayments()
        {
            _logger.LogInformation("Getting all pending payments");

            var payments = await _paymentService.GetPendingPaymentsAsync();
            return Ok(payments);
        }
    }
}