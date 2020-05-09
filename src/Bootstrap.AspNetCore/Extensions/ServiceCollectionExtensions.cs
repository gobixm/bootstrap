using Gobi.Bootstrap;
using Microsoft.Extensions.DependencyInjection;

namespace Bootstrap.AspNetCore.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBootstrap(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddSingleton<BootstrapRunner>();
        }
    }
}