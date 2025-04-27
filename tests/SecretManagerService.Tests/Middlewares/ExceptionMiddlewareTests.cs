using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using SecretManagerService.Api;

namespace SecretManagerService.Tests.Middlewares
{
    public class ExceptionMiddlewareTests
    {
        [Fact]
        public async Task ExceptionMiddleware_ShouldReturn500OnException()
        {
            // Arrange
            RequestDelegate next = (HttpContext) => throw new Exception("Test exception");
            var middleware = new ExceptionMiddleware(next);
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            // Act
            await middleware.Invoke(context);

            // Assert
            context.Response.StatusCode.Should().Be(500);
        }
    }
}