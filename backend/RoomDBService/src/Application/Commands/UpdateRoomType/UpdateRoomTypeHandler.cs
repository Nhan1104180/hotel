using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.UpdateRoomType;

public class UpdateRoomTypeHandler : IRequestHandler<UpdateRoomTypeCommand, ResponseEntity>
{
    private readonly IRoomTypeService _roomTypeService;
    public UpdateRoomTypeHandler(IRoomTypeService roomTypeService)
    {
        _roomTypeService = roomTypeService;
    }

    public async Task<ResponseEntity> Handle(UpdateRoomTypeCommand command, CancellationToken cancellationToken)
    {
        return await _roomTypeService.UpdateRoomType(command);
    }
}