using MediatR;
using Share.CommonModel;

namespace Application.Queries.SearchServices;

public class SearchServicesQuery : IRequest<ResponseEntity>
{
    public string Keyword { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}