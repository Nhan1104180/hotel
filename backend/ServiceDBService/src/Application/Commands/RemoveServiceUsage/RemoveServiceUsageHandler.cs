using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.RemoveServiceUsage;

public class RemoveServiceUsageHandler : IRequestHandler<RemoveServiceUsageCommand, ResponseEntity>
{
    private readonly IServiceUsageService _serviceUsageService;
    public RemoveServiceUsageHandler(IServiceUsageService serviceUsageService)
    {
        _serviceUsageService = serviceUsageService;
    }
    public async Task<ResponseEntity> Handle(RemoveServiceUsageCommand request, CancellationToken cancellationToken)
    {
        return await _serviceUsageService.DeleteServiceUsage(request);
    }
}