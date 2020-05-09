using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gobi.Bootstrap
{
    public interface IBootstrap
    {
        Task BootstrapAsync(
            IServiceProvider serviceProvider,
            IBootstrapState bootstrapState,
            CancellationToken cancellationToken = default);
    }
}