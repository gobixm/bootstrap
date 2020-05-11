using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Gobi.Bootstrap.AspNetCore.Middlewares;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Gobi.Bootstrap.Tests
{
    /// <summary>
    ///     <see cref="BootstrapMiddleware" />
    /// </summary>
    public sealed class BootstrapMiddlewareTests
    {
        [Fact]
        public async Task InvokeAsync_NotCompleted_503Returned()
        {
            // arrange
            var bootstrapRunner = new Mock<IBootstrapRunner>();
            bootstrapRunner.SetupGet(x => x.IsCompleted).Returns(false);
            var middleware = new BootstrapMiddleware(context => Task.CompletedTask, bootstrapRunner.Object);
            var requestContext = new DefaultHttpContext();

            // act
            await middleware.InvokeAsync(requestContext);

            // assert
            requestContext.Response.StatusCode.Should().Be(503);
        }

        [Fact]
        public async Task InvokeAsync_NotCompleted_ProgressReturned()
        {
            // arrange
            var bootstrapRunner = new Mock<IBootstrapRunner>();
            bootstrapRunner.SetupGet(x => x.IsCompleted).Returns(false);
            bootstrapRunner.SetupGet(x => x.Progress).Returns("in progress");
            var middleware = new BootstrapMiddleware(context => Task.CompletedTask, bootstrapRunner.Object);
            var requestContext = new DefaultHttpContext();
            requestContext.Response.Body = new MemoryStream();

            // act
            await middleware.InvokeAsync(requestContext);

            // assert
            requestContext.Response.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(requestContext.Response.Body);
            var responseText = await reader.ReadToEndAsync();

            responseText.Should().Be("in progress");
        }
        
        [Fact]
        public async Task InvokeAsync_Completed_StatusNotChanged()
        {
            // arrange
            var bootstrapRunner = new Mock<IBootstrapRunner>();
            bootstrapRunner.SetupGet(x => x.IsCompleted).Returns(true);
            var middleware = new BootstrapMiddleware(context => Task.CompletedTask, bootstrapRunner.Object);
            var requestContext = new DefaultHttpContext();

            // act
            await middleware.InvokeAsync(requestContext);

            // assert
            requestContext.Response.StatusCode.Should().Be(new DefaultHttpContext().Response.StatusCode);
        }
    }
}