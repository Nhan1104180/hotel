using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.CreateBooking;

public class CreateBookingHandler : IRequestHandler<CreateBookingCommand, ResponseEntity>
{
    private readonly IBookingService _bookingService;
    public CreateBookingHandler(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }
    public async Task<ResponseEntity> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        return await _bookingService.CreateBooking(request);
    }
}