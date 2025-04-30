using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SecretManagerService.Api
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            string correlationId = context.Request.Headers.TryGetValue("X-Correlation-ID", out var headerId)
                ? headerId.ToString()
                : context.TraceIdentifier;

            try
            {
                await _next(context);
                stopwatch.Stop();
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                var statusCode = MapExceptionToStatusCode(ex);
                var elapsed = stopwatch.Elapsed;

                var campos = context.Items.TryGetValue("CamposLogaveis", out var obj) && obj is Dictionary<string, string> d
                    ? d
                    : new Dictionary<string, string>();

                var problem = new CustomProblemDetails
                {
                    Type = $"https://httpstatuses.com/{statusCode}",
                    Title = GetTitle(statusCode),
                    Status = statusCode,
                    Detail = ex.Message,
                    Instance = context.Request.Path,
                    CorrelationId = correlationId,
                    TempoProcessamento = elapsed.ToString(@"hh\:mm\:ss\.fff")
                };

                var log = new ExceptionLogEntry
                {
                    DataHora = DateTime.UtcNow,
                    Rota = context.Request.Path,
                    MetodoHttp = context.Request.Method,
                    CorrelationId = correlationId,
                    Usuario = context.User?.Identity?.Name ?? "anônimo",
                    IpOrigem = context.Connection.RemoteIpAddress?.ToString(),
                    StatusCode = statusCode,
                    TipoExcecao = ex.GetType().Name,
                    MensagemErro = ex.Message,
                    TempoProcessamento = elapsed.ToString(@"hh\:mm\:ss\.fff"),
                    TempoEmMilissegundos = (long)elapsed.TotalMilliseconds,
                    StackTrace = ex.StackTrace?.Split('\n').Take(5).ToArray(),
                    CamposRequest = campos
                };

                _logger.LogError(ex, "Erro tratado {@log}", log);
                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/problem+json";
                await context.Response.WriteAsJsonAsync(problem);
            }
        }

        private static int MapExceptionToStatusCode(Exception ex)
        {
            return ex switch
            {
                KeyNotFoundException => 404,
                UnauthorizedAccessException => 401,
                ArgumentNullException => 400,
                ArgumentException => 400,
                InvalidOperationException => 409,
                NotImplementedException => 501,
                TimeoutException => 408,
                _ => 500
            };
        }

        private static string GetTitle(int statusCode) => statusCode switch
        {
            400 => "Bad Request",
            401 => "Unauthorized",
            403 => "Forbidden",
            404 => "Not Found",
            408 => "Request Timeout",
            409 => "Conflict",
            422 => "Unprocessable Entity",
            500 => "Internal Server Error",
            501 => "Not Implemented",
            _ => "Erro Inesperado"
        };
    }
}