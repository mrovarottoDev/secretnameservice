using System.Threading.Tasks;

namespace SecretManagerService.Infrastructure.Secrets
{
    public interface ISecretProvider
    {
        Task<string> GetSecretAsync(string secretName);
    }
}