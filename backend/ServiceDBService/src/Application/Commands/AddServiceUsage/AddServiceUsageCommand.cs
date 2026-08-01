using MediatR;
using Share.CommonModel;

namespace Application.Commands.AddServiceUsage;

public class AddServiceUsageCommand : IRequest<ResponseEntity>
{
    public int BookingId { get; set; }
    public int ServiceId { get; set; }
    public int Quantity { get; set; }
}