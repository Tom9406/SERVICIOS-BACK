using System.Diagnostics;

namespace Encomiendas.API.Common.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(
            RequestDelegate next,
            ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            var method = context.Request.Method;
            var path = context.Request.Path;

            var userId = context.User?.FindFirst("userId")?.Value ?? "anonymous";
            var companyId = context.User?.FindFirst("companyId")?.Value ?? "anonymous";

            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();

                var statusCode = context.Response.StatusCode;

                _logger.LogInformation(
                    "HTTP {Method} {Path} | Status: {StatusCode} | User: {UserId} | Company: {CompanyId} | Time: {Elapsed}ms",
                    method,
                    path,
                    statusCode,
                    userId,
                    companyId,
                    stopwatch.ElapsedMilliseconds
                );
            }
        }
    }
}