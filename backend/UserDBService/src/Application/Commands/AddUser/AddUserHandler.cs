using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.AddUser;

public class AddUserHandler : IRequestHandler<AddUserCommand, ResponseEntity>
{
    private readonly IUserService _userService;
    public AddUserHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<ResponseEntity> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        return await _userService.CreateUser(request);
    }
}
