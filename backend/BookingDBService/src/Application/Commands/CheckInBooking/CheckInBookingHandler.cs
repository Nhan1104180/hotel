using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.CheckInBooking;

public class CheckInBookingHandler : IRequestHandler<CheckInBookingCommand, ResponseEntity>
{
    private readonly IBookingService _bookingService;
    public CheckInBookingHandler(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }
    public async Task<ResponseEntity> Handle(CheckInBookingCommand request, CancellationToken cancellationToken)
    {
        return await _bookingService.CheckInBooking(request);
    }
}