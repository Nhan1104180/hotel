using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.AddService;

public class AddServiceHandler : IRequestHandler<AddServiceCommand, ResponseEntity>
{
    private readonly IServiceService _serviceService;
    public AddServiceHandler(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }
    public async Task<ResponseEntity> Handle(AddServiceCommand request, CancellationToken cancellationToken)
    {
        return await _serviceService.AddService(request);
    }
}