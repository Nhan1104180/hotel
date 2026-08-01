using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.PaymentCallback;

public class PaymentCallbackHandler : IRequestHandler<PaymentCallbackCommand, ResponseEntity>
{
    private readonly IPaymentService _paymentService;

    public PaymentCallbackHandler(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public async Task<ResponseEntity> Handle(PaymentCallbackCommand request, CancellationToken cancellationToken)
    {
        return await _paymentService.PaymentCallback(request);
    }
}