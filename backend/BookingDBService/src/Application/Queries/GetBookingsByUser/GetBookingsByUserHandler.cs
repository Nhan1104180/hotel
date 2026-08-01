using Application.Interfaces;
using Application.Queries.GetBookingsByUser;
using MediatR;
using Share.CommonModel;

namespace Application.Queries.GetBookingsByUserHandler;

public class GetBookingsByUserHandler : IRequestHandler<GetBookingsByUserQuery, ResponseEntity>
{
    private readonly IBookingService _bookingService;
    public GetBookingsByUserHandler(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }
    public async Task<ResponseEntity> Handle(GetBookingsByUserQuery request, CancellationToken cancellationToken)
    {
        return await _bookingService.GetBookingsByUser(request);
    }
}