using Application.Commands.UpdateRoomStatus;
using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.UpdateServiceStatus;

public class UpdateServiceStatusHandler : IRequestHandler<UpdateServiceStatusCommand, ResponseEntity>
{
    private readonly IServiceService _serviceService;

    public UpdateServiceStatusHandler(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    public async Task<ResponseEntity> Handle(UpdateServiceStatusCommand request, CancellationToken cancellationToken)
    {
        return await _serviceService.UpdateServiceStatus(request);
    }
}