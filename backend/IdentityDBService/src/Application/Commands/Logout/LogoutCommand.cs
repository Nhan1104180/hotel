using MediatR;
using Share.CommonModel;

namespace Application.Commands.Logout;

public class LogoutCommand : IRequest<ResponseEntity>
{
    public string RefreshToken { get; set; }
}