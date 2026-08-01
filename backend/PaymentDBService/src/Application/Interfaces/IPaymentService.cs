using Application.Commands.CreatePayment;
using Application.Commands.PaymentCallback;
using Application.Queries.GetPaymentById;
using Share.CommonModel;

namespace Application.Interfaces;

public interface IPaymentService
{
    Task<ResponseEntity> CreatePayment(CreatePaymentCommand command);
    Task<ResponseEntity> GetPaymentById(GetPaymentByIdQuery query);
    Task<ResponseEntity> PaymentCallback(PaymentCallbackCommand command);
}
