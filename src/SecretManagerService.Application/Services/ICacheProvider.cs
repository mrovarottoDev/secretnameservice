using System;
using System.Threading.Tasks;

namespace SecretManagerService.Application.Services
{
    public interface ICacheProvider
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan expiration);
        Task RemoveAsync(string key);
    }
}