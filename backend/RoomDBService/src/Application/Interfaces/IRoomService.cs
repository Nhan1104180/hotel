using Application.Commands.AddRoom;
using Application.Commands.AddRoomType;
using Application.Commands.RemoveRoom;
using Application.Commands.UpdateRoom;
using Application.Commands.UpdateRoomStatus;
using Application.Commands.UpdateRoomType;
using Application.Queries.GetAvailableRooms;
using Application.Queries.GetRoomById;
using Application.Queries.SeachRoom;
using Share.CommonModel;

namespace Application.Interfaces;

public interface IRoomService
{
    Task<ResponseEntity> GetAllRooms();
    Task<ResponseEntity> GetRoomById(GetRoomByIdQuery query);
    Task<ResponseEntity> AddRoom(AddRoomCommand command);
    Task<ResponseEntity> UpdateRoom(UpdateRoomCommand command);
    Task<ResponseEntity> RemoveRoom(RemoveRoomCommand command);
    Task<ResponseEntity> SearchRoom(SeachRoomQuery query);
    Task<ResponseEntity> GetAvailableRooms(GetAvailableRoomsQuery query);
    Task<ResponseEntity> UpdateRoomStatus(UpdateRoomStatusCommand command);
}