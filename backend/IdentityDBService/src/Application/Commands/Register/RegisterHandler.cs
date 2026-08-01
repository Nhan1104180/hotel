using Application.Interfaces;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.Register;

public class RegisterHandler: IRequestHandler<RegisterCommand,ResponseEntity>
{
    private readonly IAuthService _authService;

    public RegisterHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<ResponseEntity> Handle(RegisterCommand request,CancellationToken cancellationToken)
    {
        return await _authService.Register(request);
    }
}