using MediatR;
using Share.CommonModel;

namespace Application.Queries.GetBookingsByUser;

public class GetBookingsByUserQuery : IRequest<ResponseEntity>
{
    public int UserId { get; set; }
}