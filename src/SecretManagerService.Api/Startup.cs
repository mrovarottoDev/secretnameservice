using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SecretManagerService.Api
{
    public static class Startup
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddHealthChecks();
            services.AddApplicationServices(configuration);
            services.AddInfrastructureServices(configuration);
            services.AddLoggingServices();

            return services;
        }
    }
}