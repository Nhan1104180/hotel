using MediatR;
using Share.CommonModel;

namespace Application.Commands.RemoveRoom;

public class RemoveRoomCommand : IRequest<ResponseEntity>
{
    public int Id { get; set; }
}