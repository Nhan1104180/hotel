using MediatR;
using Share.CommonModel;

namespace Application.Queries.GetUserById;

public class GetUserByIdQuery : IRequest<ResponseEntity>
{
    public int Id { get; set; }
}