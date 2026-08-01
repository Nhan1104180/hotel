using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.RemoveService;

public class RemoveServiceHandler : IRequestHandler<RemoveServiceCommand, ResponseEntity>
{
    private readonly IServiceService _serviceService;
    public RemoveServiceHandler(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }
    public async Task<ResponseEntity> Handle(RemoveServiceCommand request, CancellationToken cancellationToken)
    {
        return await _serviceService.RemoveService(request);
    }
}