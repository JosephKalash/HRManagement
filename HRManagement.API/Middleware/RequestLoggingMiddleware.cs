namespace HRManagement.API.Middleware
{
    public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<RequestLoggingMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            var startTime = DateTime.UtcNow;
            var requestPath = context.Request.Path;
            var requestMethod = context.Request.Method;
            var userAgent = context.Request.Headers.UserAgent.ToString();
            var remoteIpAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            _logger.LogInformation("Request started: {Method} {Path} from {IP} - UserAgent: {UserAgent}", 
                requestMethod, requestPath, remoteIpAddress, userAgent);

            try
            {
                await _next(context);
            }
            finally
            {
                var duration = DateTime.UtcNow - startTime;
                var statusCode = context.Response.StatusCode;

                _logger.LogInformation("Request completed: {Method} {Path} - Status: {StatusCode} - Duration: {Duration}ms", 
                    requestMethod, requestPath, statusCode, duration.TotalMilliseconds);
            }
        }
    }
}
