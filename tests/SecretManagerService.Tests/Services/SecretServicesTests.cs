using Moq;
using SecretManagerService.Application.DTOs;
using SecretManagerService.Application.Services;
using SecretManagerService.Infrastructure.Secrets;
using Xunit;
using FluentAssertions;

namespace SecretManagerService.Tests.Services
{
    public class SecretServiceTests
    {
        private readonly Mock<ISecretProvider> _secretProviderMock;
        private readonly SecretService _secretService;

        public SecretServiceTests()
        {
            _secretProviderMock = new Mock<ISecretProvider>();
            _secretService = new SecretService(_secretProviderMock.Object);
        }

        [Fact]
        public async Task GetSecretAsync_ShouldReturnKeyValue_WhenSecretIsJson()
        {
            // Arrange
            var secret = "{\"username\":\"admin\",\"password\":\"12345\"}";
            _secretProviderMock.Setup(x => x.GetSecretAsync(It.IsAny<string>())).ReturnsAsync(secret);

            // Act
            var result = await _secretService.GetSecretAsync("fake-secret");

            // Assert
            result.IsKeyValue.Should().BeTrue();
            result.KeyValues.Should().ContainKey("username");
            result.KeyValues.Should().ContainKey("password");
            result.SingleValue.Should().BeNull();
        }

        [Fact]
        public async Task GetSecretAsync_ShouldReturnSingleValue_WhenSecretIsString()
        {
            // Arrange
            var secret = "plain-secret";
            _secretProviderMock.Setup(x => x.GetSecretAsync(It.IsAny<string>())).ReturnsAsync(secret);

            // Act
            var result = await _secretService.GetSecretAsync("fake-secret");

            // Assert
            result.IsKeyValue.Should().BeFalse();
            result.KeyValues.Should().BeNull();
            result.SingleValue.Should().Be(secret);
        }
    }
}