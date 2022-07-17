using AutoMapper;
using BuildingBlock.Cache.Abstraction;
using Entity.Core.Data.Abstraction;
using Entity.CQRS.Queries;
using Entity.CQRS.Queries.ResponseModels;
using Entity.ResponseModels;
using MediatR;
using System.Linq.Dynamic.Core;

namespace Entity.CQRS.Handlers.Queries
{
    public class GetFilteredQueryHandler : IRequestHandler<GetFilteredQuery, GetFilterdQueryResponseModel>
    {
        private readonly IEntityGenericRepository<Models.Entity> _repository;
        private readonly IMapper _mapper;
        private readonly IDistributedCacheStrategy _distributedCacheStrategy;

        public GetFilteredQueryHandler(
            IEntityGenericRepository<Models.Entity> repository,
            IMapper mapper,
            IDistributedCacheStrategy distributedCacheStrategy)
        {
            this._repository = repository;
            this._mapper = mapper;
            this._distributedCacheStrategy = distributedCacheStrategy;
        }
        public async Task<GetFilterdQueryResponseModel> Handle(GetFilteredQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entities = this._repository
                           .GetAll();

                if (!(string.IsNullOrEmpty(request.SortColumn) && string.IsNullOrEmpty(request.SortColumnDirection)))
                {
                    entities = entities.OrderBy(request.SortColumn + " " + request.SortColumnDirection);
                }
                if (!string.IsNullOrEmpty(request.SearchValue))
                {
                    entities = entities.Where(m => m.Id.ToString().ToLower().Contains(request.SearchValue.ToLower()));
                }

                var recordsTotal = entities.Count();

                var data = entities.Skip(request.Skip).Take(request.PageSize).ToList();

                var mappedEntities = this._mapper.Map<List<GetEntityResponseModel>>(data);

                return new GetFilterdQueryResponseModel(
                    true,
                    $"Returning: {data.ToList().Count}, entries",
                    mappedEntities);
            }
            catch (Exception ex)
            {
                return new GetFilterdQueryResponseModel(
                    false,
                    ex.Message);
            }
        }
    }
}
