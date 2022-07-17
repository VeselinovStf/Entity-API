using Entity.CQRS.Queries.ResponseModels;
using MediatR;

namespace Entity.CQRS.Queries
{
    public class GetFilteredQuery : IRequest<GetFilterdQueryResponseModel>
    {
        public GetFilteredQuery(string draw, string start, int skip, string length, string sortColumn, string sortColumnDirection, string searchValue, int pageSize, int recordsTotal)
        {
            Draw = draw;
            Start = start;
            Skip = skip;
            Length = length;
            SortColumn = sortColumn;
            SortColumnDirection = sortColumnDirection;
            SearchValue = searchValue;
            PageSize = pageSize;
            RecordsTotal = recordsTotal;
        }

        public string Draw { get; set; }
        public string Start { get; set; }
        public int Skip { get; set; }
        public string Length { get; set; }
        public string SortColumn { get; set; }
        public string SortColumnDirection { get; set; }
        public string SearchValue { get; set; }
        public int PageSize { get; set; }
        public int RecordsTotal { get; set; }
    }
}
