using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.CreatePayment;

public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand , ResponseEntity>
{
    private readonly IPaymentService _paymentService;

    public CreatePaymentHandler(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public async Task<ResponseEntity> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        return await _paymentService.CreatePayment(request);
    }
}