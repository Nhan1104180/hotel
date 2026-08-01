using MediatR;
using Share.CommonModel;

namespace Application.Queries.GetPaymentById;

public class GetPaymentByIdQuery : IRequest<ResponseEntity>
{
    public int Id { get; set; }
}