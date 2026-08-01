using Domain.Entities;
using MediatR;
using Share.CommonModel;

namespace Application.Queries.GetAllUser;

public class GetAllUserQuery : IRequest<ResponseEntity>
{
    public int pageIndex { get; set; } 

    public int PageSize { get; set; }
}

