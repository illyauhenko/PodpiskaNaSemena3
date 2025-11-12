namespace PodpiskaNaSemena.Presentation.WebHost.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var startTime = DateTime.UtcNow;

            _logger.LogInformation("Starting request {Method} {Path} at {StartTime}",
                context.Request.Method, context.Request.Path, startTime);

            try
            {
                await _next(context);

                var endTime = DateTime.UtcNow;
                var duration = endTime - startTime;

                _logger.LogInformation("Completed request {Method} {Path} with status {StatusCode} in {Duration}ms",
                    context.Request.Method, context.Request.Path, context.Response.StatusCode, duration.TotalMilliseconds);
            }
            catch (Exception ex)
            {
                var endTime = DateTime.UtcNow;
                var duration = endTime - startTime;

                _logger.LogError(ex, "Failed request {Method} {Path} with error after {Duration}ms",
                    context.Request.Method, context.Request.Path, duration.TotalMilliseconds);
                throw;
            }
        }
    }

    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LoggingMiddleware>();
        }
    }
}