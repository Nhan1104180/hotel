using MediatR;
using Share.CommonModel;

namespace Application.Queries.GetServiceUsageByBooking;

public class GetServiceUsageByBookingQuery : IRequest<ResponseEntity>
{
    public int BookingId { get; set; }
}
