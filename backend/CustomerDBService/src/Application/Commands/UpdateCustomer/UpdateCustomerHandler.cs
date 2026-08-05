using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.UpdateCustomer;

public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, ResponseEntity>
{
    private readonly ICustomerService _customerService;
    public UpdateCustomerHandler(ICustomerService customerService)
    {
        _customerService = customerService;
    }
    public async Task<ResponseEntity> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        return await _customerService.UpdateCustomer(request);
    }
}