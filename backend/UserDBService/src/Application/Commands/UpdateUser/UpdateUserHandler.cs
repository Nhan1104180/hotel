using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace UserDBService.Application.Commands.UpdateUser;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, ResponseEntity>
{
    private readonly IUserService _userService;
    public UpdateUserHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<ResponseEntity> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        return await _userService.UpdateUser(request);
    }
}