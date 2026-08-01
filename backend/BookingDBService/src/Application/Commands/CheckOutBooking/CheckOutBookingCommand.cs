using MediatR;
using Share.CommonModel;

namespace Application.Commands.CheckOutBooking;

public class CheckOutBookingCommand : IRequest<ResponseEntity>
{
    public int BookingId { get; set; }
    public DateTime CheckOutDate { get; set; }
}