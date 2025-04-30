
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        var context = _httpContextAccessor.HttpContext;
        var correlationId = context?.TraceIdentifier ?? Guid.NewGuid().ToString();

        // Extrair propriedades marcadas com [LogField]
        var campos = new Dictionary<string, string>();
        foreach (var prop in request.GetType().GetProperties())
        {
            if (Attribute.IsDefined(prop, typeof(LogFieldAttribute)))
            {
                var value = prop.GetValue(request)?.ToString();
                if (!string.IsNullOrEmpty(value))
                {
                    campos[prop.Name] = value;
                }
            }
        }

        context?.Items.TryAdd("CamposLogaveis", campos);

        _logger.LogInformation("Iniciando {Handler} | CorrelationId: {CorrelationId}", typeof(TRequest).Name, correlationId);
        var response = await next();
        stopwatch.Stop();
        _logger.LogInformation("Finalizado {Handler} em {Tempo}ms | CorrelationId: {CorrelationId}", typeof(TRequest).Name, stopwatch.ElapsedMilliseconds, correlationId);
        return response;
    }
}
