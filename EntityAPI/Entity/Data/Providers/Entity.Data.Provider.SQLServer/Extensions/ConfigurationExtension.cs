using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Entity.Core.Data.Abstraction;

namespace Entity.Data.Provider.SQLServer.Extensions
{
    public static class ConfigurationExtension
    {
        /// <summary>
        /// Add SQL Server to Application
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="dbConnectionString">DB Connection string</param>
        /// <param name="migrationAssembly">Where migratios go</param>
        public static void AddSQLServer(
            this IServiceCollection services,
            string dbConnectionString,
            string migrationAssembly)
        {
            services.AddDbContext<EntitySQLServerDbContext>(c =>
            {
                c.UseSqlServer(dbConnectionString, sqlServerOptionsAction: o => o.MigrationsAssembly(migrationAssembly));
            });

            services.AddScoped<IEntityGenericRepository<Entity.Models.Entity>, EntitySQLServerRepository>();
        }
    }
  
}

