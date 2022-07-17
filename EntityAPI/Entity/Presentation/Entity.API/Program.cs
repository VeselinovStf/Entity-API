using BuildingBlock.AppLogger.Extensions;
using BuildingBlock.DistributedCacheStrategy.Extensions;
using BuildingBlock.Utility.Abstraction;
using Entity.CQRS.Commands;
using Entity.CQRS.Extensions;
using Entity.CQRS.Queries;
using Entity.Data.Provider.SQLServer.Extensions;
using MediatR;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;

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

    var dbConnectionString = string.Empty;
    var redisConnectionString = string.Empty;

    if (builder.Environment.IsDevelopment())
    {
        dbConnectionString = builder.Configuration["DevelopmentConnectionString"];
        redisConnectionString = builder.Configuration["RedisDevelopmentConnectionString"];
    }
    else
    {
        dbConnectionString = builder.Configuration["ReleaseConnectionString"];
        redisConnectionString = builder.Configuration["RedisProductionConnectionString"];

    }

    builder.Host.ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    }).UseNLog();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Entity API",
            Description = "An Template API App",
        });
    });

    builder.Services.AddEntityCQRS();
    builder.Services.AddAppLogger();

    builder.Services.AddStackExchangeRedisCache(opt =>
    {
        opt.Configuration = redisConnectionString;
    });

    builder.Services.AddDistributedCache();

    builder.Services.AddSQLServer(dbConnectionString, "Entity.API");

    var app = builder.Build();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.MapGet("api/clients", async (IMediator mediator, IAppLogger<Program> logger) =>
    {
        var entitiesResponse = await mediator
            .Send(new GetEntitiesQuery());

        if (entitiesResponse.IsSuccess)
        {
            logger.LogInformation(entitiesResponse.Message);

            return Results.Ok(entitiesResponse);
        }
        else
        {
            logger.LogError(entitiesResponse.Message);

            return Results.BadRequest(entitiesResponse);
        }

    })
    .WithName("GetClients")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status400BadRequest);

    app.MapPost("api/clients/filter", async (
        IMediator mediator, 
        IAppLogger<Program> logger,
        GetFilteredQuery model) =>
    {
        var entitiesResponse = await mediator
            .Send(model);

        if (entitiesResponse.IsSuccess)
        {
            logger.LogInformation(entitiesResponse.Message);

            return Results.Ok(entitiesResponse);
        }
        else
        {
            logger.LogError(entitiesResponse.Message);

            return Results.BadRequest(entitiesResponse);
        }

    })
    .WithName("GetClientsFiltered")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status400BadRequest);

    app.MapGet("api/client/{id}", async (
        IMediator mediator, 
        IAppLogger<Program> logger,
        int id) =>
    {
        var entityResponse = await mediator
            .Send(new GetEntityQuery(id));

        if (entityResponse.IsSuccess)
        {
            logger.LogInformation(entityResponse.Message);

            return Results.Ok(entityResponse);
        }
        else
        {
            logger.LogError(entityResponse.Message);

            return Results.BadRequest(entityResponse);
        }

    })
    .WithName("GetClient")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status400BadRequest);

    app.MapPost("api/client/create", async (
        IMediator mediator, 
        IAppLogger<Program> logger,
        CreateEntityCommand model) =>
    {
        var entityCreateResponse = await mediator
            .Send(model);

        if (entityCreateResponse.IsSuccess)
        {
            logger.LogInformation(entityCreateResponse.Message);

            return Results.Created("api/clients",entityCreateResponse);
        }
        else
        {
            logger.LogError(entityCreateResponse.Message);

            return Results.BadRequest(entityCreateResponse);
        }

    })
    .WithName("CreateClient")
    .Produces(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest);

    app.MapPut("api/client/edit/", async (
        IMediator mediator,
        IAppLogger<Program> logger,
        UpdateEntityCommand model) =>
    {
        var entityDeleteResponse = await mediator
            .Send(model);

        if (entityDeleteResponse.IsSuccess)
        {
            logger.LogInformation(entityDeleteResponse.Message);

            return Results.Ok(entityDeleteResponse);
        }
        else
        {
            logger.LogError(entityDeleteResponse.Message);

            return Results.BadRequest(entityDeleteResponse);
        }

    })
    .WithName("UpdateClient")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status400BadRequest);

    app.MapDelete("api/client/delete/{id}", async (
        IMediator mediator,
        IAppLogger<Program> logger,
        int id) =>
    {
        var entityDeleteResponse = await mediator
            .Send(new DeleteEntityCommand(id));

        if (entityDeleteResponse.IsSuccess)
        {
            logger.LogInformation(entityDeleteResponse.Message);

            return Results.Ok(entityDeleteResponse);
        }
        else
        {
            logger.LogError(entityDeleteResponse.Message);

            return Results.BadRequest(entityDeleteResponse);
        }

    })
    .WithName("DeleteClient")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status400BadRequest);

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
