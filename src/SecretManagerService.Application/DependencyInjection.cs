using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecretManagerService.Application.Services;

namespace SecretManagerService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISecretService, SecretService>();

            return services;
        }
    }
}