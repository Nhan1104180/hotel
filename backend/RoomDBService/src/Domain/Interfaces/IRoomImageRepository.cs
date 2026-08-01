using RoomDBService.src.Domain.Entities;

namespace Domain.Interfaces;

public interface IRoomImageRepository : IRepositoryBase<RoomImage>
{
    Task RemoveRange(List<RoomImage> roomImages);
}