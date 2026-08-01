using MediatR;
using Share.CommonModel;

namespace UserDBService.Application.Commands.RemoveUser;

public class RemoveUserCommand : IRequest<ResponseEntity>
{
    public int Id { get; set; }
}