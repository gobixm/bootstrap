using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Gobi.Bootstrap.Tests
{
    /// <summary>
    ///     <see cref="BootstrapRunner" />
    /// </summary>
    public sealed class BootstrapRunnerTests
    {
        public BootstrapRunnerTests()
        {
            _bootstrapRunner = new BootstrapRunner(_serviceProvider);
        }

        private readonly Mock<IBootstrap> _bootstrapMock = new Mock<IBootstrap>();
        private readonly BootstrapRunner _bootstrapRunner;
        private readonly IServiceProvider _serviceProvider = new ServiceCollection().BuildServiceProvider();

        [Fact]
        public async Task RunAsync_Bootstrap_Called()
        {
            // assign
            var cancellationToken = new CancellationToken();

            // act
            await _bootstrapRunner.RunAsync(_bootstrapMock.Object, cancellationToken);

            // assert
            _bootstrapMock.Verify(x => x.BootstrapAsync(
                    It.IsAny<IServiceProvider>(),
                    It.IsAny<IBootstrapState>(),
                    cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task RunAsync_Exception_Propagated()
        {
            // assign
            _bootstrapMock.Setup(x => x.BootstrapAsync(
                    It.IsAny<IServiceProvider>(),
                    It.IsAny<IBootstrapState>(),
                    It.IsAny<CancellationToken>()))
                .Throws<Exception>();

            // act
            Func<Task> run = () => _bootstrapRunner.RunAsync(_bootstrapMock.Object);

            // assert
            await run.Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task RunAsync_IsCompleted_InitiallyFalse()
        {
            // assert
            _bootstrapRunner.IsCompleted.Should().BeFalse();
        }

        [Fact]
        public async Task RunAsync_IsCompleted_True()
        {
            // act
            await _bootstrapRunner.RunAsync(_bootstrapMock.Object);

            // assert
            _bootstrapRunner.IsCompleted.Should().BeTrue();
        }

        [Fact]
        public async Task RunAsync_IsCompleted_TrueOnThrow()
        {
            // assing
            _bootstrapMock.Setup(x => x.BootstrapAsync(
                    It.IsAny<IServiceProvider>(),
                    It.IsAny<IBootstrapState>(),
                    It.IsAny<CancellationToken>()))
                .Throws<Exception>();

            // act
            try
            {
                await _bootstrapRunner.RunAsync(_bootstrapMock.Object);
            }
            catch
            {
                // ignored
            }


            // assert
            _bootstrapRunner.IsCompleted.Should().BeTrue();
        }

        [Fact]
        public async Task RunAsync_Progress_Saved()
        {
            // assign
            _bootstrapMock.Setup(x => x.BootstrapAsync(
                    It.IsAny<IServiceProvider>(),
                    It.IsAny<IBootstrapState>(),
                    It.IsAny<CancellationToken>()))
                .Callback<IServiceProvider, IBootstrapState, CancellationToken>((serviceProvider, state, ct) =>
                {
                    state.Progress = "progress";
                });

            // act
            await _bootstrapRunner.RunAsync(_bootstrapMock.Object);

            // assert
            _bootstrapRunner.Progress.Should().Be("progress");
        }

        [Fact]
        public async Task RunAsync_Progress_Updated()
        {
            // assign
            _bootstrapMock.Setup(x => x.BootstrapAsync(
                    It.IsAny<IServiceProvider>(),
                    It.IsAny<IBootstrapState>(),
                    It.IsAny<CancellationToken>()))
                .Callback<IServiceProvider, IBootstrapState, CancellationToken>((serviceProvider, state, ct) =>
                {
                    state.Progress = "last";
                    state.Progress = "win";
                });

            // act
            await _bootstrapRunner.RunAsync(_bootstrapMock.Object);

            // assert
            _bootstrapRunner.Progress.Should().Be("win");
        }
    }
}