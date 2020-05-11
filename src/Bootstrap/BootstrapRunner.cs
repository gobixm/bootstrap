using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Gobi.Bootstrap
{
    public sealed class BootstrapRunner : IBootstrapRunner
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IBootstrapState _state = new BootstrapState();

        public BootstrapRunner(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public bool IsCompleted { get; private set; }

        public string Progress => _state.Progress;

        public async Task RunAsync(IBootstrap bootstrap, CancellationToken cancellationToken = default)
        {
            using var scope = _serviceProvider.CreateScope();
            try
            {
                await bootstrap.BootstrapAsync(scope.ServiceProvider, _state, cancellationToken);
            }
            finally
            {
                IsCompleted = true;
            }
        }
    }
}