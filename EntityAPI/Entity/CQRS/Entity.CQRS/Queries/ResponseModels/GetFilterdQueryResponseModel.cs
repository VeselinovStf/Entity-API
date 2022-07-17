using Entity.ResponseModels;

namespace Entity.CQRS.Queries.ResponseModels
{
    public class GetFilterdQueryResponseModel : BaseResponse<IEnumerable<GetEntityResponseModel>>
    {
        public GetFilterdQueryResponseModel(bool isSuccess, string message) : base(isSuccess, message)
        {
        }

        public GetFilterdQueryResponseModel(bool isSuccess, string message, IEnumerable<GetEntityResponseModel> data) : base(isSuccess, message, data)
        {
        }
    }
}
