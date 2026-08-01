using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Queries.GetPaymentById;

public class GetPaymentByIdHandler : IRequestHandler<GetPaymentByIdQuery, ResponseEntity>
{
    private readonly IPaymentService _paymentService;

    public GetPaymentByIdHandler(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public async Task<ResponseEntity> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
    {
        return await _paymentService.GetPaymentById(request);
    }
}