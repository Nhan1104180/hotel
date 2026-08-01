using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Queries.GetRoomById;

public class GetRoomByIdHandler : IRequestHandler<GetRoomByIdQuery, ResponseEntity>
{
    private readonly IRoomService _roomService;

    public GetRoomByIdHandler(IRoomService roomService)
    {
        _roomService = roomService;
    }

    public async Task<ResponseEntity> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
    {
        return await _roomService.GetRoomById(request);
    }
}