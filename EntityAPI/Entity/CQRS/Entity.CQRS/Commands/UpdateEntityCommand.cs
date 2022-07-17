using Entity.CQRS.Commands.ResponseModels;
using MediatR;

namespace Entity.CQRS.Commands
{
    public class UpdateEntityCommand : IRequest<UpdateEntityCommandResponseModel>
    {
        public UpdateEntityCommand(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}
