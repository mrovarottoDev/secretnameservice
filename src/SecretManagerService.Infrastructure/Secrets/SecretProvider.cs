using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using System;
using System.Threading.Tasks;

namespace SecretManagerService.Infrastructure.Secrets
{
    public class SecretProvider : ISecretProvider
    {
        private readonly IAmazonSecretsManager _secretsManager;

        public SecretProvider(IAmazonSecretsManager secretsManager)
        {
            _secretsManager = secretsManager;
        }

        public async Task<string> GetSecretAsync(string secretName)
        {
            var response = await _secretsManager.GetSecretValueAsync(new GetSecretValueRequest
            {
                SecretId = secretName
            });

            if (response.SecretString != null)
            {
                return response.SecretString;
            }

            throw new Exception("Secret not found or inaccessible.");
        }
    }
}