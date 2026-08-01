using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Queries.GetServiceUsageByBooking;

public class GetServiceUsageByBookingHandler : IRequestHandler<GetServiceUsageByBookingQuery, ResponseEntity>
{
    private readonly IServiceUsageService _serviceUsageService;

    public GetServiceUsageByBookingHandler(IServiceUsageService serviceUsageService)
    {
        _serviceUsageService = serviceUsageService;
    }

    public async Task<ResponseEntity> Handle(GetServiceUsageByBookingQuery request, CancellationToken cancellationToken)
    {
        return await _serviceUsageService.GetServiceUsageByBookingId(request);
    }
}