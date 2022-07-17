using Entity.Core.Data.Abstraction;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Entity.CQRS.Extensions
{
    public static class ConfigurationExtension
    {
        public static void AddEntityCQRS(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}
