using Entity.ResponseModels;

namespace Entity.CQRS.Commands.ResponseModels
{
    public class CreateEntityCommandResponseModel : BaseResponse<GetEntityResponseModel>
    {
        public CreateEntityCommandResponseModel(bool isSuccess, string message) : base(isSuccess, message)
        {
        }

        public CreateEntityCommandResponseModel(bool isSuccess, string message, GetEntityResponseModel data) : base(isSuccess, message, data)
        {
        }
    }
}
