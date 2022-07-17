
using BuildingBlock.Utility.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlock.AppLogger.Extensions
{
    public static class ConfigurationExtension
    {
        public static void AddAppLogger(this IServiceCollection services)
        {
            services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));
        }
    }
}
