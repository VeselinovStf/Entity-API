using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((appConfiguration) =>
                    {
                        appConfiguration.AddJsonFile("appsettings.json", optional: true);
                    });

                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureLogging((whbc, logging) =>
                    {
                        logging.ClearProviders();
                        logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    });
                });
    }
}
