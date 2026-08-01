using Application.Commands.Login;
using Application.Commands.Logout;
using Application.Commands.Register;
using Application.DTO;
using AutoMapper;
using IdentityDBService.Application.Commands.RefreshToken;
namespace Application.Mapping;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
       CreateMap<RegisterDTO,RegisterCommand>();
       CreateMap<LoginDTO,LoginCommand>();
       CreateMap<RefreshTokenRequestDTO, RefreshTokenCommand>();
       CreateMap<LogoutRequestDTO, LogoutCommand>();
    }
}