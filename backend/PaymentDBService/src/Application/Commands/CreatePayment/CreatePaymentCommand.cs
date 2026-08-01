using MediatR;
using Share.CommonModel;

namespace Application.Commands.CreatePayment;

public class CreatePaymentCommand : IRequest<ResponseEntity>
{
    public int BookingId { get; set; }
    public decimal Amount { get; set; }
    public int PaymentMethodId { get; set; }
    public string? Description { get; set; }
}