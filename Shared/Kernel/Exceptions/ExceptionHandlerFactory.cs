using Microsoft.Extensions.Hosting;
using FluentValidation;
using System.Net;

namespace Ecommerce.Shared.Contract.Exceptions
{
    public interface IExceptionHandler
    {
        bool CanHandle(Exception exception);
        (HttpStatusCode StatusCode, string Message) Handle(Exception exception, IHostEnvironment env);
    }

    public abstract class BaseExceptionHandler<T> : IExceptionHandler where T : Exception
    {
        public virtual bool CanHandle(Exception exception) => exception is T;

        public abstract (HttpStatusCode StatusCode, string Message) Handle(Exception exception, IHostEnvironment env);
    }

    public class ValidationExceptionHandler : BaseExceptionHandler<ValidationException>
    {
        public override (HttpStatusCode StatusCode, string Message) Handle(Exception exception, IHostEnvironment env)
        {
            var validationEx = (ValidationException)exception;
            var message = env.IsDevelopment()
                ? $"Validation failed: {string.Join(", ", validationEx.Errors.Select(e => e.ErrorMessage))}"
                : "Validation failed";

            return (HttpStatusCode.BadRequest, message);
        }
    }

    public class DomainExceptionHandler : BaseExceptionHandler<DomainException>
    {
        public override (HttpStatusCode StatusCode, string Message) Handle(Exception exception, IHostEnvironment env)
        {
            var domainEx = (DomainException)exception;

            var statusCode = domainEx switch
            {
                NotFoundException => HttpStatusCode.NotFound,
                BadRequestException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };

            return (statusCode, domainEx.Message);
        }
    }

    public class DefaultExceptionHandler : IExceptionHandler
    {
        public bool CanHandle(Exception exception) => true; // Handles all exceptions as fallback

        public (HttpStatusCode StatusCode, string Message) Handle(Exception exception, IHostEnvironment env)
        {
            var statusCode = exception switch
            {
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                ArgumentException or ArgumentNullException => HttpStatusCode.BadRequest,
                TimeoutException => HttpStatusCode.RequestTimeout,
                NotSupportedException => HttpStatusCode.NotImplemented,
                _ => HttpStatusCode.InternalServerError
            };

            var message = env.IsDevelopment() ? exception.Message : "An unexpected error occurred.";
            return (statusCode, message);
        }
    }

    public class ExceptionHandlerFactory
    {
        private readonly List<IExceptionHandler> _handlers;

        public ExceptionHandlerFactory()
        {
            _handlers = new List<IExceptionHandler>
            {
                new ValidationExceptionHandler(),
                new DomainExceptionHandler(),
                new DefaultExceptionHandler()
            };
        }

        public IExceptionHandler GetHandler(Exception exception)
        {
            return _handlers.First(handler => handler.CanHandle(exception));
        }
    }
}
