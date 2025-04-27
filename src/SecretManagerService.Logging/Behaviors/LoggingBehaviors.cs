using MediatR;
using Serilog;
using SecretManagerService.Logging.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace SecretManagerService.Logging.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var stopwatch = Stopwatch.StartNew();
            var logEntry = new LogEntry
            {
                RequestName = requestName,
                Request = request
            };

            try
            {
                var response = await next();
                stopwatch.Stop();

                logEntry.Success = true;
                logEntry.ElapsedTime = stopwatch.Elapsed;

                Log.Information("Request completed successfully {@LogEntry}", logEntry);

                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                logEntry.Success = false;
                logEntry.ElapsedTime = stopwatch.Elapsed;
                logEntry.ErrorMessage = ex.Message;

                Log.Error(ex, "Request failed {@LogEntry}", logEntry);

                throw;
            }
        }
    }
}