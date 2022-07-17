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
    public class DeleteEntityCommandHandler : IRequestHandler<DeleteEntityCommand, DeleteEntityCommandResponseModel>
    {
        private readonly IEntityGenericRepository<Models.Entity> _repository;
        private readonly IMapper _mapper;
        private readonly IDistributedCacheStrategy _distributedCacheStrategy;
        public DeleteEntityCommandHandler(
           IEntityGenericRepository<Models.Entity> repository,
           IMapper mapper,
           IDistributedCacheStrategy distributedCacheStrategy)
        {
            this._repository = repository;
            this._mapper = mapper;
            this._distributedCacheStrategy = distributedCacheStrategy;
        }

        public async Task<DeleteEntityCommandResponseModel> Handle(DeleteEntityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await this._repository.Get(request.Id);

                if (entity != null)
                {
                    entity.IsDeleted = true;

                    await this._repository.SaveChangesAsync();

                    var mappedEntity = this._mapper.Map<GetEntityResponseModel>(entity);

                    await this._distributedCacheStrategy.Remove(CacheKey.GetAllEntities);

                    return new DeleteEntityCommandResponseModel(true, "Entity Deleted!", mappedEntity);
                }

                return new DeleteEntityCommandResponseModel(false, "Can't Find Entity");
            }
            catch (Exception ex)
            {
                return new DeleteEntityCommandResponseModel(
                  false,
                  ex.Message);
            }
        }
    }
}
