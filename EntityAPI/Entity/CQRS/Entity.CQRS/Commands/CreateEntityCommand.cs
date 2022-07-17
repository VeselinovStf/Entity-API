using Entity.CQRS.Commands.ResponseModels;
using MediatR;

namespace Entity.CQRS.Commands
{
    public record CreateEntityCommand : IRequest<CreateEntityCommandResponseModel> 
    {
        public CreateEntityCommand()
        {

        }
    }
}
