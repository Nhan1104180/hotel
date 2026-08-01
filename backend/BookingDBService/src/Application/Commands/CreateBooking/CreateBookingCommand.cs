using Domain.ValueObjects;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.CreateBooking;

public class CreateBookingCommand : IRequest<ResponseEntity>
{
    public int RoomId { get; set; }
    public int CustomerId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
}