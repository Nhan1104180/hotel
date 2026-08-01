using MediatR;
using Share.CommonModel;

namespace Application.Commands.AddRoomType;

public class AddRoomTypeCommand : IRequest<ResponseEntity>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Capacity { get; set; }
}