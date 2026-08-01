using MediatR;
using Share.CommonModel;

namespace Application.Queries.GetBookingById;

public class GetBookingByIdQuery : IRequest<ResponseEntity>
{
    public int Id { get; set; }
}