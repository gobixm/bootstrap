using System;
using System.Threading;
using System.Threading.Tasks;
using Bootstrap.AspNetCore.Middlewares;
using Gobi.Bootstrap;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bootstrap.AspNetCore.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseBootstrap(this IApplicationBuilder applicationBuilder,
            IBootstrap bootstrap)
        {
            var lifetime = applicationBuilder.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
            var runner = applicationBuilder.ApplicationServices.GetRequiredService<BootstrapRunner>();

            var cs = new CancellationTokenSource();

            lifetime.ApplicationStarted.Register(() => runner.RunAsync(bootstrap, cs.Token));
            lifetime.ApplicationStopped.Register(() => cs.Cancel());

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