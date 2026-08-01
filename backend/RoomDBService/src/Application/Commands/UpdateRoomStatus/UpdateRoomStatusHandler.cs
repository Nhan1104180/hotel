using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.UpdateRoomStatus;

public class UpdateRoomStatusHandler : IRequestHandler<UpdateRoomStatusCommand,ResponseEntity>
{
    private readonly IRoomService _roomService;

    public UpdateRoomStatusHandler(IRoomService roomService)
    {
        _roomService = roomService;
    }

    public async Task<ResponseEntity> Handle(UpdateRoomStatusCommand request, CancellationToken cancellationToken)
    {
        return await _roomService.UpdateRoomStatus(request);
    }
}