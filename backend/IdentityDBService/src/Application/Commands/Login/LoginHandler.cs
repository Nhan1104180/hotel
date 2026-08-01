using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.Login;

public class LoginHandler : IRequestHandler<LoginCommand, ResponseEntity>
{
    private readonly IAuthService _authService;

    public LoginHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<ResponseEntity> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return await _authService.Login(request);
    }
}