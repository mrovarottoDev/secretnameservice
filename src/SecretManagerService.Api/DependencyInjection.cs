using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SecretManagerService.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddHealthChecks();
            return services;
        }
    }
}