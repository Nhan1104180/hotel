using MediatR;
using Share.CommonModel;

namespace Application.Commands.RemoveServiceUsage;

public class RemoveServiceUsageCommand : IRequest<ResponseEntity>
{

    public int BookingId { get ;set; }
    public int ServiceId { get ;set;}
}