using Application.Commands.AddService;
using Application.Commands.UpdateRoomStatus;
using Application.Commands.UpdateService;
using Application.DTO;
using AutoMapper;

namespace Application.Mapping;

public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        CreateMap<AddServiceDTO, AddServiceCommand>();
        CreateMap<UpdateServiceDTO, UpdateServiceCommand>();
        CreateMap<UpdateStatusDTO, UpdateServiceStatusCommand>();
    }
}