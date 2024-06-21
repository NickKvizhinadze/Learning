using System.Diagnostics;
using System.Reflection;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace RiverBooks.SharedKernel;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) :
    IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);

        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("Handling {RequestName}", typeof(TRequest).Name);
            
            Type myType = request.GetType();
            var props = new List<PropertyInfo>(myType.GetProperties());
            foreach (var prop in props)
            {
                object? propValue = prop?.GetValue(request, null);
                logger.LogInformation("Property {Property} : {Value}", prop?.Name, propValue);
            }
        }

        var sw = Stopwatch.StartNew();
        var response = await next();

        logger.LogInformation("Handled {RequestName} with {Response} in {ms} ms",
            typeof(TRequest).Name, response, sw.ElapsedMilliseconds);

        return response;
    }
}