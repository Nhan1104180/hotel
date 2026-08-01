using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.Logout;

public class LogoutHandler : IRequestHandler<LogoutCommand, ResponseEntity>
{
    private readonly IAuthService _authService;
    public LogoutHandler(IAuthService authService)
    {
        _authService = authService;
    }
    public async Task<ResponseEntity> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        return await _authService.Logout(request);
    }
}