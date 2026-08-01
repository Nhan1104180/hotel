using Application.Commands.AddRoomType;
using Application.Commands.RemoveRoomType;
using Application.Commands.UpdateRoomType;
using Share.CommonModel;

namespace Application.Interfaces;

public interface IRoomTypeService
{
    Task<ResponseEntity> GetAllRoomType();
    Task<ResponseEntity> AddRoomType(AddRoomTypeCommand command);
    Task<ResponseEntity> UpdateRoomType(UpdateRoomTypeCommand command);
    Task<ResponseEntity> RemoveRoomType(RemoveRoomTypeCommand command);
}
