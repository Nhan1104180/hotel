using Application.Commands.AddUser;
using Application.DTO;
using AutoMapper;
using UserDBService.Application.Commands.UpdateUser;

namespace Application.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserCreateDTO, AddUserCommand>();
        CreateMap<UserUpdateDTO, UpdateUserCommand>();
    }
}
