using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Queries.SearchServices;

public class SearchServicesHandler : IRequestHandler<SearchServicesQuery, ResponseEntity>
{
    private readonly IServiceService _serviceService;

    public SearchServicesHandler(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    public async Task<ResponseEntity> Handle(SearchServicesQuery request, CancellationToken cancellationToken)
    {
        return await _serviceService.SearchService(request);
    }
}