using MediatR;
using Share.CommonModel;

namespace Application.Queries.GetAllRooms;

public class GetAllRoomsQuery : IRequest<ResponseEntity>;
