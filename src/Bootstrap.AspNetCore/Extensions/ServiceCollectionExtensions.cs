using Microsoft.Extensions.DependencyInjection;

namespace Gobi.Bootstrap.AspNetCore.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBootstrap(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddSingleton<IBootstrapRunner, BootstrapRunner>();
        }
    }
}