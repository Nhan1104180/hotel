using Application.Commands.AddRoom;
using Application.Commands.AddRoomType;
using Application.Commands.UpdateRoom;
using Application.Commands.UpdateRoomStatus;
using Application.Commands.UpdateRoomType;
using Application.DTO;
using AutoMapper;

namespace Application.Mapping;

public class RoomProfile : Profile
{
    public RoomProfile()
    {
        CreateMap<AddRoomTypeDTO, AddRoomTypeCommand>();
        CreateMap<AddRoomDTO, AddRoomCommand>();
        CreateMap<UpdateRoomDTO, UpdateRoomCommand>();
        CreateMap<UpdateRoomTypeDTO, UpdateRoomTypeCommand>();
        CreateMap<UpdateRoomStatusDTO, UpdateRoomStatusCommand>();
    }
}