using BookingDBService.src.Domain.Entities;
using Domain.ValueObjects;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.CheckInBooking;

public class CheckInBookingCommand : IRequest<ResponseEntity>
{
    public int BookingId { get; set; }
    public DateTime CheckInDate { get; set; }
}
