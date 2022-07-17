using BuildingBlock.Cache.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlock.DistributedCacheStrategy.Extensions
{
    public static class ConfigurationExtension
    {
        public static void AddDistributedCache(this IServiceCollection services)
        {
            services.AddSingleton<IDistributedCacheStrategy, DistributedCacheStrategy>();
        }
    }
}
