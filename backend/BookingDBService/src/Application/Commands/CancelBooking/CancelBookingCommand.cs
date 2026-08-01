using MediatR;
using Share.CommonModel;

namespace Application.Commands.CancelBooking;

public class CancelBookingCommand : IRequest<ResponseEntity>
{
    public int BookingId { get; set; }
}