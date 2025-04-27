using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SecretManagerService.Logging.Behaviors;

namespace SecretManagerService.Logging
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddLoggingServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            return services;
        }
    }
}