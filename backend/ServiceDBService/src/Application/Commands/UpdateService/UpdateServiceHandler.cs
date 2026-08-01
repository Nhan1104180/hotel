using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.UpdateService;

public class UpdateServiceHandler : IRequestHandler<UpdateServiceCommand, ResponseEntity>
{
    private readonly IServiceService _serviceService;

    public UpdateServiceHandler(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    public async Task<ResponseEntity> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        return await _serviceService.UpdateService(request);
    }
}