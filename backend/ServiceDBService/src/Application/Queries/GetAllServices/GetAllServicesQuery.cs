using MediatR;
using Share.CommonModel;

namespace Application.Queries.GetAllServices;

public record GetAllServicesQuery : IRequest<ResponseEntity>;