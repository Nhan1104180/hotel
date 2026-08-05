using Application.Interfaces;
using Domain.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Queries.GetCustomerById;

public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdQuery, ResponseEntity>
{
    private readonly ICustomerService _customerService;
    public GetCustomerByIdHandler(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task<ResponseEntity> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        return await _customerService.GetCustomerById(request);
    }
}