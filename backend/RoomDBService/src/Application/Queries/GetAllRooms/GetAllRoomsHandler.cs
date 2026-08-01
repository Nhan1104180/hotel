using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Queries.GetAllRooms;

public class GetAllRoomsHandler : IRequestHandler<GetAllRoomsQuery, ResponseEntity>
{
    private readonly IRoomService _roomService;
    public GetAllRoomsHandler(IRoomService roomService)
    {
        _roomService = roomService;
    }
    public async Task<ResponseEntity> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
    {
        return await _roomService.GetAllRooms();
    }
}