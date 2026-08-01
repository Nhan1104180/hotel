using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.RemoveRoomType;

public class RemoveRoomTypeHandler : IRequestHandler<RemoveRoomTypeCommand, ResponseEntity>
{
    private readonly IRoomTypeService _roomTypeService;
    public RemoveRoomTypeHandler(IRoomTypeService roomTypeService)
    {
        _roomTypeService = roomTypeService;
    }
    public async Task<ResponseEntity> Handle(RemoveRoomTypeCommand request, CancellationToken cancellationToken)
    {
        return await _roomTypeService.RemoveRoomType(request);
    }
}