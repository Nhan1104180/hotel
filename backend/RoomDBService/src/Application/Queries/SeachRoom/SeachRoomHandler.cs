using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Queries.SeachRoom;

public class SeachRoomHandler : IRequestHandler<SeachRoomQuery, ResponseEntity>
{
    private readonly IRoomService _roomService;
    public SeachRoomHandler(IRoomService roomService)
    {
        _roomService = roomService;
    }
    public async Task<ResponseEntity> Handle(SeachRoomQuery request, CancellationToken cancellationToken)
    {
        return await _roomService.SearchRoom(request);
    }
}