using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.UpdateRoom;

public class UpdateRoomHandler : IRequestHandler<UpdateRoomCommand, ResponseEntity>
{
    private readonly IRoomService _roomService;

    public UpdateRoomHandler(IRoomService roomService)
    {
        _roomService = roomService;
    }

    public async Task<ResponseEntity> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
    {
        return await _roomService.UpdateRoom(request);
    }
}