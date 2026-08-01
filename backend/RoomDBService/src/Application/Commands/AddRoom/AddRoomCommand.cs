using MediatR;
using Share.CommonModel;

namespace Application.Commands.AddRoom;

public class AddRoomCommand : IRequest<ResponseEntity>
{
    public string RoomNumber { get; set; }
    public int RoomTypeId { get; set; }
    public decimal Price { get; set; } 
}