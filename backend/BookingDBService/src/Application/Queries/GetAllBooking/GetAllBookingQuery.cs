using MediatR;
using Share.CommonModel;

namespace Application.Queries.GetAllBooking;

public class GetAllBookingQuery : IRequest<ResponseEntity>
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; }  = 10;
}