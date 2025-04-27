using SecretManagerService.Application.DTOs;
using System.Threading.Tasks;

namespace SecretManagerService.Application.Services
{
    public interface ISecretService
    {
        Task<SecretResult> GetSecretAsync(string secretName);
    }
}