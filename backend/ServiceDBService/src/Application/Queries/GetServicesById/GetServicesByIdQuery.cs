using MediatR;
using Share.CommonModel;

namespace Application.Queries.GetServicesById;

public class GetServicesByIdQuery : IRequest<ResponseEntity>
{
    public int Id { get; set; }
}