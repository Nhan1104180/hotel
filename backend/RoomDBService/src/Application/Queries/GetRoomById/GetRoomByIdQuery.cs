using MediatR;
using Share.CommonModel;

namespace Application.Queries.GetRoomById;

public class GetRoomByIdQuery : IRequest<ResponseEntity>
{
    public int Id { get; set; }
}