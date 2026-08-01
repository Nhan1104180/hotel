using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Queries.GetAvailableRooms;

public class GetAvailableRoomsHandler : IRequestHandler<GetAvailableRoomsQuery, ResponseEntity>
{
    private readonly IRoomService _roomService;
    public GetAvailableRoomsHandler(IRoomService roomService)
    {
        _roomService = roomService;
    }
    public async Task<ResponseEntity> Handle(GetAvailableRoomsQuery request, CancellationToken cancellationToken)
    {
        return await _roomService.GetAvailableRooms(request);
    }
        
    
}