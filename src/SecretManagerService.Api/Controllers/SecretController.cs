using Microsoft.AspNetCore.Mvc;
using SecretManagerService.Application.Services;

namespace SecretManagerService.Api.Controllers
{
    [ApiController]
    [Route("api/secrets")]
    public class SecretController : ControllerBase
    {
        private readonly ISecretService _secretService;

        public SecretController(ISecretService secretService)
        {
            _secretService = secretService;
        }

        [HttpGet("{secretName}")]
        public async Task<IActionResult> GetSecret(string secretName)
        {
            var result = await _secretService.GetSecretAsync(secretName);
            return Ok(result);
        }
    }
}