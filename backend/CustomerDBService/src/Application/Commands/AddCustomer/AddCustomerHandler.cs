using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.AddCustomer;

public class AddCustomerHandler : IRequestHandler<AddCustomerCommand, ResponseEntity>
{
    private readonly ICustomerService _customerService;
    public AddCustomerHandler(ICustomerService customerService)
    {
        _customerService = customerService;
    }
    public async Task<ResponseEntity> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
    {
        return await _customerService.CreateCustomer(request);
    }
}
