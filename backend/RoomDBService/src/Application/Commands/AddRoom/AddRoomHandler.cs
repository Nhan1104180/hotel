using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.AddRoom;

public class AddRoomHandler : IRequestHandler<AddRoomCommand, ResponseEntity>
{
    private readonly IRoomService _roomService;
    public AddRoomHandler(IRoomService roomService)
    {
        _roomService = roomService;
    }
    public async Task<ResponseEntity> Handle(AddRoomCommand request, CancellationToken cancellationToken)
    {
        return await _roomService.AddRoom(request);
    }
}