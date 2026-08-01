using MediatR;
using Share.CommonModel;

namespace Application.Queries.GetAvailableRooms;

public class GetAvailableRoomsQuery : IRequest<ResponseEntity>
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}