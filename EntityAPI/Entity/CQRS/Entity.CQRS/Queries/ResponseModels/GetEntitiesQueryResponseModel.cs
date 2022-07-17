using Entity.ResponseModels;

namespace Entity.CQRS.Queries.ResponseModels
{
    public class GetEntitiesQueryResponseModel : BaseResponse<IEnumerable<GetEntityResponseModel>>
    {
        public GetEntitiesQueryResponseModel(bool isSuccess, string message) : base(isSuccess, message)
        {
        }

        public GetEntitiesQueryResponseModel(bool isSuccess, string message, IEnumerable<GetEntityResponseModel> data) : base(isSuccess, message, data)
        {
        }
    }
}
