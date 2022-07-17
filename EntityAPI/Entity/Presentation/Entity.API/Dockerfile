#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Entity/Presentation/Entity.API/Entity.API.csproj", "Entity/Presentation/Entity.API/"]
COPY ["Entity/CQRS/Entity.CQRS/Entity.CQRS.csproj", "Entity/CQRS/Entity.CQRS/"]
COPY ["Entity/Core/Entity.Core.Data.Abstraction/Entity.Core.Data.Abstraction.csproj", "Entity/Core/Entity.Core.Data.Abstraction/"]
COPY ["BuildingBlocks/Core/BuildingBlock.Cache.Abstraction/BuildingBlock.Cache.Abstraction.csproj", "BuildingBlocks/Core/BuildingBlock.Cache.Abstraction/"]
COPY ["Entity/Data/Models/Entity.Models/Entity.Models.csproj", "Entity/Data/Models/Entity.Models/"]
COPY ["BuildingBlocks/Utility/BuildingBlock.AppLogger/BuildingBlock.AppLogger.csproj", "BuildingBlocks/Utility/BuildingBlock.AppLogger/"]
COPY ["BuildingBlocks/Core/BuildingBlock.Utility.Abstraction/BuildingBlock.Utility.Abstraction.csproj", "BuildingBlocks/Core/BuildingBlock.Utility.Abstraction/"]
COPY ["BuildingBlocks/Cache/BuildingBlock.DistributedCacheStrategy/BuildingBlock.DistributedCacheStrategy.csproj", "BuildingBlocks/Cache/BuildingBlock.DistributedCacheStrategy/"]
COPY ["Entity/Data/Providers/Entity.Data.Provider.SQLServer/Entity.Data.Provider.SQLServer.csproj", "Entity/Data/Providers/Entity.Data.Provider.SQLServer/"]
RUN dotnet restore "Entity/Presentation/Entity.API/Entity.API.csproj"
COPY . .
WORKDIR "/src/Entity/Presentation/Entity.API"
RUN dotnet build "Entity.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Entity.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Entity.API.dll"]