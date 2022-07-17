# Entity-API

![simple representation](https://github.com/VeselinovStf/Entity-API/blob/main/repo/img.png)

Building blocks for creating API/Microservice projects

## Tech Stack

- ASP .NET CORE 3.1
- .NET 6
- Identity Server 4
- EntityFrameworkCore 6
- SQL Server
- RabbitMQ
- Redis
- Ocelot
- MediatR
- AutoMapper
- Swagger
- NLog
- Docker


## Entity API Template

### Environment Set Up

### .env File
  - Create environment file
```
DB_VOLUME_APPDATA= Database volume path
DB_PASSWORD= Database password
RABBITMQ_DEFAULT_VHOST=CUSTOM_HOST
RABBITMQ_DEFAULT_USER=myuser
RABBITMQ_DEFAULT_PASS=mypassword
```

- Bring up containers
  
```
docker-compose -f .\docker-compose-db.yml up -d --build
docker-compose -f .\docker-compose-mq.yml up -d --build
docker-compose -f .\docker-compose-redis.yml up -d --build
docker-compose -f .\docker-compose.yml up -d --build
```

## Parts

### API Gateway

- APIGateway.OcelotGateway
    - Goal: Provide a unified point of entry into the system.
    - Implementation: Api Gateway is implemented using .NET 6 API Template using Ocelot 18.0. 
    - Access: Gateway endpoints are secured through JWT token Authentication

### Building Blocks

- Cache
  - BuildingBlock.DistributedCacheStrategy
    - Goal:  Improve the performance and scalability of API Services that make use of it.
    - Implementation: Functionality is provided through BuildingBlock.Cache.Abstraction.IDistributedCacheStrategy interface
- Core
  - Goal: Provide unified Abstraction layer for all Building Blocks
    - BuildingBlock.Cache.Abstraction
    - BuildingBlock.Messaging.Abstraction
      - Event Bus - implement business transactions that span multiple services, which give you eventual consistency between those services.
      - RabbitMQ
    - BuildingBlock.Utility.Abstraction
      - AppLogger - Application Logging Abstraction
- Messaging
  - Goal: Create Event bus proof-of-concept implementation
    - BuildingBlock.EventBusRabboitMQ 
    - BuildingBlock.EventBus
- Utility
  - BuildingBlock.AppLogger

### Entity Blocks

- Core
  - Entity.Core.Data.Abstraction
    - Database Abstraction
- CQRS
  - Commands (saves) and queries (reads) segregation block.
    - Entity.CQRS
      - GOAL: Create 'Home' for CRUD operations performed by Presentation Layer. Separate logic from View. Establish single source of trough for business logic. Create a layer that may be used in API/MVC/MVVM based apps.  
- Data
  - Models
    - Entity.Models
      - Database Models
  - Providers
    - Entity.Core.Data.Abstraction Implementations for different Database providers
      - Entity.Data.Fake
        - Fake Database access layer
      - Entity.Data.Provider.SQLServer
        - SQL Server Database access layer
- Presentation
  - API Service Implementation
    - Entity.API - made use of other building blocks. Implements RESTfull API.

### Services Blocks

- IdentityService
    - Implements IdentityServer4 framework for ASP.NET Core 3.1. Serves as proof-of-concept implementation for identity layer.
## Endpoints

### Identity Endpoint

- POST: http://localhost:9999/token

```json
[
    {
        "key":"username",
        "value":"admin",
        "description":""
    },
    {
        "key":"password",
        "value":"!Aa12345678",
        "description":""
    },
    {
        "key":"grant_type",
        "value":"client_credentials",
        "description":""
    },
    {
        "key":"client_id",
        "value":"admin",
        "description":""
    },
    {
        "key":"client_secret",
        "value":"!Aa12345678",
        "description":""
    }
]
```

- GET: http://localhost:9999/clients
- GET: http://localhost:9999/clients/{id}
- POST: http://localhost:9999/clients
- PUT: http://localhost:9999/clients
- DELETE: http://localhost:9999/clients