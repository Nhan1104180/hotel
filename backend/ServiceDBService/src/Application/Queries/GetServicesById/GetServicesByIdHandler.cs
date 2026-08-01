using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Queries.GetServicesById;

public class GetServicesByIdHandler : IRequestHandler<GetServicesByIdQuery, ResponseEntity>
{
    private readonly IServiceService _serviceService;

    public GetServicesByIdHandler(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    public async Task<ResponseEntity> Handle(GetServicesByIdQuery request, CancellationToken cancellationToken)
    {
        return await _serviceService.GetServiceById(request);
    }
}