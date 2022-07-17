using NLog;
using NLog.Web;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;

var logsPath = Path.Combine(Environment.CurrentDirectory, "Logs");

NLog.GlobalDiagnosticsContext.Set("AppDirectory", logsPath);
NLog.Common.InternalLogger.LogFile = Path.Combine(logsPath, "nlog-internal.log");

// NLog: setup the logger first to catch all errors
var logger = NLog.LogManager.Setup()
    .RegisterNLogWeb().GetCurrentClassLogger();

try
{
    logger.Debug("Init main");


    var builder = WebApplication.CreateBuilder(args);
    builder.Host.ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    }).UseNLog();

    IdentityModelEventSource.ShowPII = true;

    builder.Host.ConfigureAppConfiguration((hc, config) =>
    {
        config
            .SetBasePath(hc.HostingEnvironment.ContentRootPath)
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{hc.HostingEnvironment.EnvironmentName}.json", true, true)
            .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"ocelot.{hc.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    });

    var authenticationProviderKey = "identityAPIService";
    var authority = "http://localhost:6666";

    if (builder.Environment.IsDevelopment()) 
    {
        authority = "http://identityserver"; //  Container
    }

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy",
            builder => builder
            .SetIsOriginAllowed((host) => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
    });


    builder.Services.AddAuthentication()
        .AddJwtBearer(authenticationProviderKey, x =>
                 {
                     x.Authority = authority; // IDENTITY SERVER URL
                                              //x.RequireHttpsMetadata = false;
                     x.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateAudience = false,
                         ValidateIssuer = false,           // TODO: try with true     

                     };

                     x.RequireHttpsMetadata = false;
                 });


    builder.Services.AddOcelot();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseCors("CorsPolicy");
    app.UseOcelot().Wait();
    app.Run();

}
catch (Exception ex)
{
    //NLog: catch setup errors
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}
