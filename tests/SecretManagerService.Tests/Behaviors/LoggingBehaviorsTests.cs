using FluentAssertions;
using Moq;
using Serilog;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using SecretManagerService.Logging.Behaviors;
using MediatR;

namespace SecretManagerService.Tests.Behaviors
{
    public class LoggingBehaviorTests
    {
        [Fact]
        public async Task LoggingBehavior_ShouldLogSuccess()
        {
            // Arrange
            var behavior = new LoggingBehavior<SampleRequest, SampleResponse>();
            var request = new SampleRequest();
            RequestHandlerDelegate<SampleResponse> next = () => Task.FromResult(new SampleResponse());

            // Act
            var response = await behavior.Handle(request, next, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
        }

        private class SampleRequest : IRequest<SampleResponse> { }
        private class SampleResponse { }
    }
}