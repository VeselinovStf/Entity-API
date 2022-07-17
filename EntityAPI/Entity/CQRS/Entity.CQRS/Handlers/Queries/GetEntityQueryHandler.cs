using AutoMapper;
using BuildingBlock.Cache.Abstraction;
using Entity.Core.Data.Abstraction;
using Entity.CQRS.Const;
using Entity.CQRS.Queries;
using Entity.CQRS.Queries.ResponseModels;
using Entity.ResponseModels;
using MediatR;

namespace Entity.CQRS.Handlers.Queries
{
    public class GetEntityQueryHandler : IRequestHandler<GetEntityQuery, GetEntityQueryResponseModel>
    {

        private readonly IEntityGenericRepository<Models.Entity> _repository;
        private readonly IMapper _mapper;
        private readonly IDistributedCacheStrategy _distributedCacheStrategy;

        public GetEntityQueryHandler(
            IEntityGenericRepository<Models.Entity> repository,
            IMapper mapper,
            IDistributedCacheStrategy distributedCacheStrategy)
        {
            this._repository = repository;
            this._mapper = mapper;
            this._distributedCacheStrategy = distributedCacheStrategy;
        }
        public async Task<GetEntityQueryResponseModel> Handle(GetEntityQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entitiesQuery = await this._distributedCacheStrategy
                  .GetAsync<Entity.Models.Entity>(CacheKey.GetAllEntities, async () =>
                  {
                      return await this._repository
                           .Get(request.Id);
                  });

                var mappedEntities = this._mapper.Map<GetEntityResponseModel>(entitiesQuery);

                return new GetEntityQueryResponseModel(
                    true,
                    $"Returning entry",
                    mappedEntities);
            }
            catch (Exception ex)
            {
                return new GetEntityQueryResponseModel(
                    false,
                    ex.Message);
            }
        }
    }
}
