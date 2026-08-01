using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.CancelBooking;

public class CancelBookingHandler : IRequestHandler<CancelBookingCommand, ResponseEntity>
{
    private readonly IBookingService _bookingService;
    public CancelBookingHandler(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }
    public async Task<ResponseEntity> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
    {
        return await _bookingService.CancelBooking(request);
    }
}