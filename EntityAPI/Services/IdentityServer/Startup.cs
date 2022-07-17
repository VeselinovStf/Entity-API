using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Reflection;

namespace IdentityServer
{
    public class Startup
    {
        public Startup(
             IConfiguration configuration,
             IWebHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            Environment = hostEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = "";
            if (this.Environment.IsDevelopment())
            {
                connectionString = Configuration["DevelopmentConnectionString"];
            }
            else
            {
                connectionString = Configuration["ReleaseConnectionString"];
            }

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            // configure identity server with in-memory stores, keys, clients and scopes
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                // this adds the config data from DB (clients, resources)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 30;
                });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .WithOrigins("http://localhost:3000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddSingleton<ICorsPolicyService>((container) =>
            {
                var logger = container.GetRequiredService<ILogger<DefaultCorsPolicyService>>();
                return new DefaultCorsPolicyService(logger)
                {
                    AllowedOrigins = { "http://localhost:3000" }
                };
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // this will do the initial DB population
            InitializeDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Identity Server.");
                });
            });
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            var section = Configuration.GetSection("IdentityConfig");

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in Config.GetClients(section))
                    {
                        context.Clients.Add(client.ToEntity());
                    }

                    context.SaveChanges();
                }

                //if (!context.IdentityResources.Any())
                //{
                //    foreach (var resource in Config.GetIdentityResources())
                //    {
                //        context.IdentityResources.Add(resource.ToEntity());
                //    }
                //    context.SaveChanges();
                //}

                if (!context.ApiScopes.Any())
                {
                    foreach (var resource in Config.GetScopes())
                    {
                        context.ApiScopes.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Config.GetApiResources(section))
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
