using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.RemoveRoom;

public class RemoveRoomHandler : IRequestHandler<RemoveRoomCommand, ResponseEntity>
{
    private readonly IRoomService _roomService;
    public RemoveRoomHandler(IRoomService roomService)
    {
        _roomService = roomService;
    }
    public async Task<ResponseEntity> Handle(RemoveRoomCommand request, CancellationToken cancellationToken)
    {
        return await _roomService.RemoveRoom(request);
    }
}