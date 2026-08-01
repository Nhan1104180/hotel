using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.CheckOutBooking;

public class CheckOutBookingHandler : IRequestHandler<CheckOutBookingCommand, ResponseEntity>
{
    private readonly IBookingService _bookingService;
    public CheckOutBookingHandler(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }
    public async Task<ResponseEntity> Handle(CheckOutBookingCommand request, CancellationToken cancellationToken)
    {
        return await _bookingService.CheckOutBooking(request);
    }
}