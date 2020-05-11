using System;
using System.Threading;
using System.Threading.Tasks;
using Gobi.Bootstrap.AspNetCore.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Gobi.Bootstrap.AspNetCore.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseBootstrap(this IApplicationBuilder applicationBuilder,
            IBootstrap bootstrap)
        {
            var lifetime = applicationBuilder.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
            var runner = applicationBuilder.ApplicationServices.GetRequiredService<IBootstrapRunner>();

            lifetime.ApplicationStarted.Register(() => runner.RunAsync(bootstrap, lifetime.ApplicationStopped));

            return applicationBuilder.UseMiddleware<BootstrapMiddleware>();
        }

        public static IApplicationBuilder UseBootstrap(this IApplicationBuilder applicationBuilder,
            Func<IServiceProvider, IBootstrapState, CancellationToken, Task> bootstrap)
        {
            var delegateBootstrap = new DelegateBootstrap(bootstrap);
            return applicationBuilder.UseBootstrap(delegateBootstrap);
        }
    }
}