using Entity.CQRS.Commands.ResponseModels;
using MediatR;

namespace Entity.CQRS.Commands
{
    public class DeleteEntityCommand : IRequest<DeleteEntityCommandResponseModel>
    {
        public DeleteEntityCommand(int id)
        {
            Id = id;
        }
        public int Id { get;}
    }
}
