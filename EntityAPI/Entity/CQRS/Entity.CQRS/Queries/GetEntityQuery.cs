using Entity.CQRS.Queries.ResponseModels;
using MediatR;

namespace Entity.CQRS.Queries
{
    public class GetEntityQuery : IRequest<GetEntityQueryResponseModel>
    {
        public GetEntityQuery(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}
