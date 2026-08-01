using Application.DTO;
using RoomDBService.src.Domain.Entities;

namespace Domain.Interfaces;

public interface IRoomRepository : IRepositoryBase<Room>
{
    Task<List<RoomDTO>> GetAllRoomsAsync();
    Task<RoomDTO> GetRoomByIdAsync(int id);
    Task<Room?> GetRoomEntityByIdAsync(int id);
    Task<bool> ExistsByRoomNumberAsync(string roomNumber);
    Task<Room> AddRoomAsync(Room room);
    Task<List<RoomDTO>> SearchRoomAsync(string keyword, int pageIndex, int pageSize);
    Task<List<AvailableRoomDTO>> GetAvailableRoomsAsync(int pageIndex, int pageSize);
}