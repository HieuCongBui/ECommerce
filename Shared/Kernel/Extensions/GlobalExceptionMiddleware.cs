using Ecommerce.Shared.Contract.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Ecommerce.Shared.Contract.Extensions
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        private readonly ExceptionHandlerFactory _exceptionHandlerFactory;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
            _exceptionHandlerFactory = new ExceptionHandlerFactory();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _logger, _env);
            }
        }

        private async Task HandleExceptionAsync(
            HttpContext context,
            Exception ex,
            ILogger logger,
            IHostEnvironment env)
        {
            var handler = _exceptionHandlerFactory.GetHandler(ex);
            var (statusCode, message) = handler.Handle(ex, env);

            logger.LogError(ex, "Exception caught in middleware: {ExceptionType} - {Message}",
                ex.GetType().Name, ex.Message);

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var errorResponse = new
            {
                statusCode = (int)statusCode,
                message,
                traceId = context.TraceIdentifier,
                timestamp = DateTime.UtcNow,
                path = context.Request.Path.Value,
                details = env.IsDevelopment() ? ex.StackTrace : null,
                innerException = env.IsDevelopment() && ex.InnerException != null ? ex.InnerException.Message : null
            };

            var json = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = env.IsDevelopment()
            });

            await context.Response.WriteAsync(json);
        }
    }
}
