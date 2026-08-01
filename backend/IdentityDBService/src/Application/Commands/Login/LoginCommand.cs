using MediatR;
using Share.CommonModel;

namespace Application.Commands.Login;

public class LoginCommand : IRequest<ResponseEntity>
{
    public string EmailOrUsernameOrPhone { get; set; }
    public string Password { get; set; }
}