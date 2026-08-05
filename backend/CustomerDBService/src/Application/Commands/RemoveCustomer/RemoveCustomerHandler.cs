using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.RemoveCustomer;

public class RemoveCustomerHandler : IRequestHandler<RemoveCustomerCommand, ResponseEntity>
{
    private readonly ICustomerService _customerService;
    public RemoveCustomerHandler(ICustomerService customerService)
    {
        _customerService = customerService;
    }
    public async Task<ResponseEntity> Handle(RemoveCustomerCommand request, CancellationToken cancellationToken)
    {
        return await _customerService.DeleteCustomer(request);
    }
}