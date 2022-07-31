using BuildingBlock.Utility.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlock.AppLogger.Extensions
{
    public static class ConfigurationExtension
    {
        /// <summary>
        /// Add Application Logging to Application
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddAppLogger(this IServiceCollection services)
        {
            services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));
        }
    }
}
