using System.Net;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace Encomiendas.API.Common.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (SqlException ex)
            {
                await HandleSqlException(context, ex);
            }
            catch (Exception)
            {
                await HandleGenericException(context);
            }
        }

        private static async Task HandleSqlException(HttpContext context, SqlException ex)
        {
            var statusCode = HttpStatusCode.BadRequest;

            // Mapear errores THROW de tu SP
            switch (ex.Number)
            {
                case 50001: // Sender = Receiver
                case 50002: // Branch igual
                    statusCode = HttpStatusCode.BadRequest;
                    break;

                case 50003: // Sender not found
                case 50004: // Receiver not found
                    statusCode = HttpStatusCode.NotFound;
                    break;

                case 50005: // Tracking no inicializado
                case 50006: // Estado inicial no configurado
                    statusCode = HttpStatusCode.InternalServerError;
                    break;

                default:
                    statusCode = HttpStatusCode.BadRequest;
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                error = ex.Message,
                code = ex.Number
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private static async Task HandleGenericException(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;

            var response = new
            {
                error = "Internal server error"
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}