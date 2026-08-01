using RoomDBService.src.Domain.Entities;

namespace Domain.Interfaces;

public interface IRoomTypeRepository : IRepositoryBase<RoomType>
{
    Task<List<RoomType>> GetAllRoomTypeAsync();
    Task<RoomType> GetRoomTypeAsync(int roomTypeId);
    Task<bool> ExistsByNameAsync(string name,int? excludeId = null);
    Task<RoomType> AddRoomTypeAsync(RoomType roomType);
}