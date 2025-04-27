using Amazon.SecretsManager;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecretManagerService.Infrastructure.Cache;
using SecretManagerService.Infrastructure.Secrets;
// using StackExchange.Redis; // Descomentar se quiser usar Redis

namespace SecretManagerService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAWSService<IAmazonSecretsManager>();

            // MemoryCache como padrão
            services.AddMemoryCache();
            services.AddScoped<ICacheProvider, MemoryCacheProvider>();

            /*
            // Caso queira usar Redis no lugar do MemoryCache:
            var redisConnectionString = configuration.GetConnectionString("Redis");
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));
            services.AddScoped<ICacheProvider, RedisCacheProvider>();
            */

            services.AddScoped<ISecretProvider, SecretProvider>();

            return services;
        }
    }
}