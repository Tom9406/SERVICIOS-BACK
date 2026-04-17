using System.Net;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace Encomiendas.API.Common.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(
            RequestDelegate next,
            ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path;
            var method = context.Request.Method;

            var userId = context.User?.FindFirst("userId")?.Value ?? "anonymous";
            var companyId = context.User?.FindFirst("companyId")?.Value ?? "anonymous";

            try
            {
                await _next(context);
            }
            catch (SqlException ex)
            {
                _logger.LogWarning(ex,
                    "SQL Error | {Method} {Path} | User: {UserId} | Company: {CompanyId}",
                    method, path, userId, companyId);

                await HandleSqlException(context, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex,
                    "Unauthorized | {Method} {Path} | User: {UserId}",
                    method, path, userId);

                await HandleUnauthorized(context, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unhandled Error | {Method} {Path} | User: {UserId} | Company: {CompanyId}",
                    method, path, userId, companyId);

                await HandleGenericException(context);
            }
        }
        private static async Task HandleSqlException(HttpContext context, SqlException ex)
        {
            var statusCode = ex.Number switch
            {
                60001 => HttpStatusCode.Forbidden,      
                60004 => HttpStatusCode.NotFound,        
                60010 => HttpStatusCode.Unauthorized,
                50001 or 50002 => HttpStatusCode.BadRequest,
                50003 or 50004 => HttpStatusCode.NotFound,
                50005 or 50006 => HttpStatusCode.InternalServerError,
                _ => HttpStatusCode.BadRequest
            };

            await WriteResponse(context, statusCode, ex.Number, ex.Message);
        }

        private static async Task HandleUnauthorized(HttpContext context, Exception ex)
        {
            await WriteResponse(
                context,
                HttpStatusCode.Unauthorized,
                401,
                ex.Message
            );
        }

        private static async Task HandleGenericException(HttpContext context)
        {
            await WriteResponse(
                context,
                HttpStatusCode.InternalServerError,
                500,
                "Internal server error"
            );
        }

        private static async Task WriteResponse(
            HttpContext context,
            HttpStatusCode statusCode,
            int code,
            string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                success = false,
                data = (object?)null,
                error = new
                {
                    code,
                    message
                }
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}