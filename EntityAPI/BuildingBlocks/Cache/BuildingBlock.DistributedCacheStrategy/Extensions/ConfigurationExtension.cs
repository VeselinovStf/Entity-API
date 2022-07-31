using BuildingBlock.Cache.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlock.DistributedCacheStrategy.Extensions
{
    public static class ConfigurationExtension
    {
        /// <summary>
        /// Add Dustributed Cache to Application
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddDistributedCache(this IServiceCollection services)
        {
            services.AddSingleton<IDistributedCacheStrategy, DistributedCacheStrategy>();
        }
    }
}
