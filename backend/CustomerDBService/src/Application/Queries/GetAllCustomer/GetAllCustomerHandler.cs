using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Queries.GetAllCustomer;

public class GetAllCustomerHandler : IRequestHandler<GetAllCustomerQuery, ResponseEntity>
{
    private readonly ICustomerService _customerService;
    public GetAllCustomerHandler(ICustomerService customerService)
    {
        _customerService = customerService;
    }
    public async Task<ResponseEntity> Handle(GetAllCustomerQuery request, CancellationToken cancellationToken)
    {
        return await _customerService.GetAllCustomer(request);
    }

}