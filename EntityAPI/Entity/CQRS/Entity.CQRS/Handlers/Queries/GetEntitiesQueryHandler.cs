using AutoMapper;
using BuildingBlock.Cache.Abstraction;
using Entity.Core.Data.Abstraction;
using Entity.CQRS.Const;
using Entity.CQRS.Queries;
using Entity.CQRS.Queries.ResponseModels;
using Entity.ResponseModels;
using MediatR;

namespace Entity.API.CQRS.Handlers.Queries
{
    public class GetEntitiesQueryHandler : IRequestHandler<GetEntitiesQuery, GetEntitiesQueryResponseModel>
    {
        private readonly IEntityGenericRepository<Models.Entity> _repository;
        private readonly IMapper _mapper;
        private readonly IDistributedCacheStrategy _distributedCacheStrategy;

        public GetEntitiesQueryHandler(
            IEntityGenericRepository<Models.Entity> repository,
            IMapper mapper,
            IDistributedCacheStrategy distributedCacheStrategy)
        {
            this._repository = repository;
            this._mapper = mapper;
            this._distributedCacheStrategy = distributedCacheStrategy;
        }
        public async Task<GetEntitiesQueryResponseModel> Handle(GetEntitiesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entitiesQuery = await this._distributedCacheStrategy
                  .GetListAsync<Entity.Models.Entity>(CacheKey.GetAllEntities, async () =>
                  {
                      return this._repository
                           .GetAll();
                  });
                
                var mappedEntities = this._mapper.Map<List<GetEntityResponseModel>>(entitiesQuery);

                return new GetEntitiesQueryResponseModel(
                    true,
                    $"Returning: {entitiesQuery.ToList().Count}, entries",
                    mappedEntities);
            }
            catch (Exception ex)
            {
                return new GetEntitiesQueryResponseModel(
                    false,
                    ex.Message);
            }
        }
    }
}
