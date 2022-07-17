using Entity.ResponseModels;

namespace Entity.CQRS.Commands.ResponseModels
{
    public class UpdateEntityCommandResponseModel : BaseResponse<GetEntityResponseModel>
    {
        public UpdateEntityCommandResponseModel(bool isSuccess, string message) : base(isSuccess, message)
        {
        }

        public UpdateEntityCommandResponseModel(bool isSuccess, string message, GetEntityResponseModel data) : base(isSuccess, message, data)
        {
        }
    }
}
