using SecretManagerService.Application.DTOs;
using SecretManagerService.Application.Services;
using SecretManagerService.Application.Settings;
using SecretManagerService.Infrastructure.Secrets;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace SecretManagerService.Application.Services
{
    public class SecretService : ISecretService
    {
        private readonly ISecretProvider _secretProvider;
        private readonly ICacheProvider _cacheProvider;

        public SecretService(ISecretProvider secretProvider, ICacheProvider cacheProvider)
        {
            _secretProvider = secretProvider;
            _cacheProvider = cacheProvider;
        }

        public async Task<SecretResult> GetSecretAsync(string secretName)
        {
            var cachedSecret = await _cacheProvider.GetAsync<SecretResult>(secretName);
            if (cachedSecret != null)
                return cachedSecret;

            var secretValue = await _secretProvider.GetSecretAsync(secretName);

            SecretResult result;
            if (IsJson(secretValue))
            {
                var keyValues = JsonSerializer.Deserialize<Dictionary<string, string>>(secretValue);
                result = new SecretResult
                {
                    IsKeyValue = true,
                    KeyValues = keyValues
                };
            }
            else
            {
                result = new SecretResult
                {
                    IsKeyValue = false,
                    SingleValue = secretValue
                };
            }

            await _cacheProvider.SetAsync(secretName, result, TimeSpan.FromMinutes(CacheSettings.DefaultExpirationInMinutes));
            return result;
        }

        private bool IsJson(string input)
        {
            input = input.Trim();
            return (input.StartsWith("{") && input.EndsWith("}"));
        }
    }
}