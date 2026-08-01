using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Queries.GetAllServices;

public class GetAllServicesHandler : IRequestHandler<GetAllServicesQuery, ResponseEntity>
{
    private readonly IServiceService _serviceService;

    public GetAllServicesHandler(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    public async Task<ResponseEntity> Handle(GetAllServicesQuery request, CancellationToken cancellationToken)
    {
        return await _serviceService.GetAllServices();
    }
}