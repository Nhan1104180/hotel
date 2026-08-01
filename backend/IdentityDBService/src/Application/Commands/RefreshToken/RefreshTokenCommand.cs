using MediatR;
using Share.CommonModel;

namespace IdentityDBService.Application.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<ResponseEntity>
{
    public string RefreshToken { get; set; } = null!;
}