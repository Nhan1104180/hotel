using MediatR;
using Share.CommonModel;

namespace Application.Commands.UpdateRoom;

public class UpdateRoomCommand : IRequest<ResponseEntity>
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public int RoomTypeId { get; set; }
}