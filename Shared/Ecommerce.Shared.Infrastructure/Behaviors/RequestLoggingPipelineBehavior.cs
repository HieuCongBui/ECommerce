
using Ecommerce.Shared.Contract.Commons;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Ecommerce.Shared.Infrastructure.Behaviors
{
    public sealed class RequestLoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
    where TResponse : Result
    {
        private readonly ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> _logger;

        public RequestLoggingPipelineBehavior(ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;

            _logger.LogInformation("Processing request {RequestName}", requestName);
            TResponse response = await next();
            if (response.IsSuccess)
            {
                _logger.LogInformation("Request {RequestName} processed successfully", requestName);
            }
            else
            {
                using (LogContext.PushProperty("Error", response.Error, true))
                {
                    _logger.LogError("Request {RequestName} processed with errors", requestName);
                }
            }

            return response;
        }
    }
}
