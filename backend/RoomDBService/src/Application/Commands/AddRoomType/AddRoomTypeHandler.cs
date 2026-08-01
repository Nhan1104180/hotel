using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.AddRoomType;

public class AddRoomTypeHandler : IRequestHandler<AddRoomTypeCommand, ResponseEntity>
{
    private readonly IRoomTypeService _roomTypeService;
    public AddRoomTypeHandler(IRoomTypeService roomTypeService)
    {
        _roomTypeService = roomTypeService;
    }

    public async Task<ResponseEntity> Handle(AddRoomTypeCommand request, CancellationToken cancellationToken)
    {
       return await _roomTypeService.AddRoomType(request);
    }
}