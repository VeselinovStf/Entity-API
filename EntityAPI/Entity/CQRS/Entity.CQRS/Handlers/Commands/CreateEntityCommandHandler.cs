using AutoMapper;
using BuildingBlock.Cache.Abstraction;
using Entity.Core.Data.Abstraction;
using Entity.CQRS.Commands;
using Entity.CQRS.Commands.ResponseModels;
using Entity.CQRS.Const;
using Entity.ResponseModels;
using MediatR;

namespace Entity.CQRS.Handlers.Commands
{
    public class CreateEntityCommandHandler : IRequestHandler<CreateEntityCommand, CreateEntityCommandResponseModel>
    {
        private readonly IEntityGenericRepository<Models.Entity> _repository;
        private readonly IMapper _mapper;
        private readonly IDistributedCacheStrategy _distributedCacheStrategy;
        public CreateEntityCommandHandler(
           IEntityGenericRepository<Models.Entity> repository,
           IMapper mapper,
           IDistributedCacheStrategy distributedCacheStrategy)
        {
            this._repository = repository;
            this._mapper = mapper;
            this._distributedCacheStrategy = distributedCacheStrategy;
        }

        public async Task<CreateEntityCommandResponseModel> Handle(CreateEntityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var addedEntity = await this._repository.AddAsync(new Models.Entity());

                if (addedEntity != null)
                {
                    await this._repository.SaveChangesAsync();

                    var mappedEntity = this._mapper.Map<GetEntityResponseModel>(addedEntity);

                    await this._distributedCacheStrategy.Remove(CacheKey.GetAllEntities);

                    return new CreateEntityCommandResponseModel(true, "Entity Created!", mappedEntity);
                }

                return new CreateEntityCommandResponseModel(false, "Can't Create Entity");
            }
            catch (Exception ex)
            {
                return new CreateEntityCommandResponseModel(
                  false,
                  ex.Message);
            }
        }
    }
}
