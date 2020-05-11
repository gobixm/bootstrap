using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Gobi.Bootstrap.Tests
{
    /// <summary>
    ///     <see cref="DelegateBootstrap" />
    /// </summary>
    public sealed class DelegateBootstrapTests
    {
        [Fact]
        public async Task BootstrapAsync_Delegate_Called()
        {
            // arrange
            var bootstrapAction = new Mock<Func<IServiceProvider, IBootstrapState, CancellationToken, Task>>();
            var bootstrap = new DelegateBootstrap(bootstrapAction.Object);
            var serviceProvider = new ServiceCollection().BuildServiceProvider();
            var bootstrapStateMock = new Mock<IBootstrapState>();

            // act
            await bootstrap.BootstrapAsync(serviceProvider, bootstrapStateMock.Object, CancellationToken.None);

            // assert
            bootstrapAction.Verify(bootstrapCallback => bootstrapCallback(
                    serviceProvider,
                    bootstrapStateMock.Object,
                    CancellationToken.None),
                Times.Once);
        }
    }
}