using Application.Commands.Login;
using Application.Commands.Logout;
using Application.Commands.Register;
using IdentityDBService.Application.Commands.RefreshToken;
using Share.CommonModel;

namespace Application.Interfaces;

public interface IAuthService
{
    //đăng ký
    Task<ResponseEntity> Register(RegisterCommand command);
    //đăng nhập
    Task<ResponseEntity>Login(LoginCommand login);
    //lấy token mới
    Task<ResponseEntity> RefreshToken(RefreshTokenCommand command);
    //đăng xuất
    Task<ResponseEntity> Logout(LogoutCommand command);
}