using AutoMapper;
using BuildingBlock.Cache.Abstraction;
using Entity.Core.Data.Abstraction;
using Entity.CQRS.Commands;
using Entity.CQRS.Commands.ResponseModels;
using Entity.CQRS.Const;
using Entity.ResponseModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.CQRS.Handlers.Commands
{
    public class UpdateEntityCommandHandler : IRequestHandler<UpdateEntityCommand, UpdateEntityCommandResponseModel>
    {
        private readonly IEntityGenericRepository<Models.Entity> _repository;
        private readonly IMapper _mapper;
        private readonly IDistributedCacheStrategy _distributedCacheStrategy;
        public UpdateEntityCommandHandler(
           IEntityGenericRepository<Models.Entity> repository,
           IMapper mapper,
           IDistributedCacheStrategy distributedCacheStrategy)
        {
            this._repository = repository;
            this._mapper = mapper;
            this._distributedCacheStrategy = distributedCacheStrategy;
        }

        public async Task<UpdateEntityCommandResponseModel> Handle(UpdateEntityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await this._repository.Get(request.Id);

                if (entity != null)
                {
                    // Update Fields
                    entity.ModifiedAt = DateTime.Now;

                    await this._repository.SaveChangesAsync();

                    var mappedEntity = this._mapper.Map<GetEntityResponseModel>(entity);

                    await this._distributedCacheStrategy.Remove(CacheKey.GetAllEntities);

                    return new UpdateEntityCommandResponseModel(true, "Entity Updated!", mappedEntity);
                }

                return new UpdateEntityCommandResponseModel(false, "Can't Find Entity");
            }
            catch (Exception ex)
            {
                return new UpdateEntityCommandResponseModel(
                  false,
                  ex.Message);
            }
        }
    }
}
