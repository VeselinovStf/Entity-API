using Entity.ResponseModels;

namespace Entity.CQRS.Queries.ResponseModels
{
    public class GetEntityQueryResponseModel : BaseResponse<GetEntityResponseModel>
    {
        public GetEntityQueryResponseModel(bool isSuccess, string message) : base(isSuccess, message)
        {
        }

        public GetEntityQueryResponseModel(bool isSuccess, string message, GetEntityResponseModel data) : base(isSuccess, message, data)
        {
        }
    }
}
