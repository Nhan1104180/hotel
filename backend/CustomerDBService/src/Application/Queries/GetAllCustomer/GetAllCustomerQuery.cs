using MediatR;
using Share.CommonModel;

namespace Application.Queries.GetAllCustomer;

public class GetAllCustomerQuery : IRequest<ResponseEntity>
{
    public int pageIndex { get; set; } 
    public int PageSize { get; set; }
}

