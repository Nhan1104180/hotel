using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace UserDBService.Application.Commands.RemoveUser;

public class RemoveUserHandler : IRequestHandler<RemoveUserCommand, ResponseEntity>
{
    private readonly IUserService _userService;
    public RemoveUserHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<ResponseEntity> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
    {
        return await _userService.DeleteUser(request);
    }
}