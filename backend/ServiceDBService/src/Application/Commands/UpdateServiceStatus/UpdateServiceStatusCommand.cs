using Domain.Enums;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.UpdateRoomStatus;

public class UpdateServiceStatusCommand : IRequest<ResponseEntity>
{
    public int Id { get; set; }
    public ServiceStatus Status { get; set; }
}