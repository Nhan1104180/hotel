using Application.Interfaces;
using IdentityDBService.Application.Commands.RefreshToken;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.RefreshToken;

public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, ResponseEntity>
{
    private readonly IAuthService _authService;
    public RefreshTokenHandler(IAuthService authService)
    {
        _authService = authService;
    }
    public async Task<ResponseEntity> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        return await _authService.RefreshToken(request);
    }
}