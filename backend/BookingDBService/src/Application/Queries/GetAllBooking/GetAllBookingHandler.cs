using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Queries.GetAllBooking;

public class GetAllBookingHandler : IRequestHandler<GetAllBookingQuery, ResponseEntity>
{
    private readonly IBookingService _bookingService;

    public GetAllBookingHandler(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public async Task<ResponseEntity> Handle(GetAllBookingQuery request, CancellationToken cancellationToken)
    {
        return await _bookingService.GetAllBooking(request);
    }
}