using System;
using System.Threading.Tasks;

namespace SecretManagerService.Infrastructure.Cache
{
    public interface ICacheProvider
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan expiration);
        Task RemoveAsync(string key);
    }
}