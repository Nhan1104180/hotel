using MediatR;
using Share.CommonModel;

namespace Application.Queries.GetCustomerById;

public class GetCustomerByIdQuery : IRequest<ResponseEntity>
{
    public int Id { get; set; }
}