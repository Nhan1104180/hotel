using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Queries.GetBookingById;

public class GetBookingByIdHandler : IRequestHandler<GetBookingByIdQuery, ResponseEntity>
{
    private readonly IBookingService _bookingService;
    public GetBookingByIdHandler(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public async Task<ResponseEntity> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        return await _bookingService.GetBookingById(request);
    }
}