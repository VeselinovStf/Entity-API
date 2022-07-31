using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Entity.CQRS.Extensions
{
    public static class ConfigurationExtension
    {
        /// <summary>
        /// Adds CQRS to Application
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddEntityCQRS(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}
