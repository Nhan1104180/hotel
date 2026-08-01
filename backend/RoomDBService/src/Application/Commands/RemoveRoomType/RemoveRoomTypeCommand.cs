using MediatR;
using Share.CommonModel;

namespace Application.Commands.RemoveRoomType;

public class RemoveRoomTypeCommand : IRequest<ResponseEntity>
{
    public int Id { get; set; }
}

