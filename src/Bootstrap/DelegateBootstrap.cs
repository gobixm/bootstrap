using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gobi.Bootstrap
{
    public sealed class DelegateBootstrap : IBootstrap
    {
        private readonly Func<IServiceProvider, IBootstrapState, CancellationToken, Task> _bootstrap;

        public DelegateBootstrap(Func<IServiceProvider, IBootstrapState, CancellationToken, Task> bootstrap)
        {
            _bootstrap = bootstrap ?? throw new ArgumentNullException(nameof(bootstrap));
        }

        public async Task BootstrapAsync(IServiceProvider serviceProvider, IBootstrapState bootstrapState,
            CancellationToken cancellationToken = default)
        {
            await _bootstrap(serviceProvider, bootstrapState, cancellationToken);
        }
    }
}