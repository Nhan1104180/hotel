using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.AddServiceUsage;

public class AddServiceUsageHandler : IRequestHandler<AddServiceUsageCommand, ResponseEntity>
{
    private readonly IServiceUsageService _serviceUsageService;

    public AddServiceUsageHandler(IServiceUsageService serviceUsageService)
    {
        _serviceUsageService = serviceUsageService;
    }

    public async Task<ResponseEntity> Handle(AddServiceUsageCommand request, CancellationToken cancellationToken)
    {
        return await _serviceUsageService.AddServiceUsage(request);
    }
}