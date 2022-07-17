using Entity.CQRS.Queries.ResponseModels;
using MediatR;

namespace Entity.CQRS.Queries
{
    public record GetEntitiesQuery : IRequest<GetEntitiesQueryResponseModel>
    {
    }
}
