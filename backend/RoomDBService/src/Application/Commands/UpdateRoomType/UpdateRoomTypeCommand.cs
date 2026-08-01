using MediatR;
using Share.CommonModel;

namespace Application.Commands.UpdateRoomType;

public class UpdateRoomTypeCommand : IRequest<ResponseEntity>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Capacity { get; set; }
}