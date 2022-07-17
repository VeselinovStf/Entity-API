using Entity.ResponseModels;

namespace Entity.CQRS.Commands.ResponseModels
{
    public class DeleteEntityCommandResponseModel : BaseResponse<GetEntityResponseModel>
    {
        public DeleteEntityCommandResponseModel(bool isSuccess, string message) : base(isSuccess, message)
        {
        }

        public DeleteEntityCommandResponseModel(bool isSuccess, string message, GetEntityResponseModel data) : base(isSuccess, message, data)
        {
        }
    }
}
