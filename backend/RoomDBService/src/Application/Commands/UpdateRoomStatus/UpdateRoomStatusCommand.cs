using Domain.Enums;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.UpdateRoomStatus;

public class UpdateRoomStatusCommand : IRequest<ResponseEntity>
{
    public int Id { get; set; }
    public RoomStatus Status { get; set; }
}