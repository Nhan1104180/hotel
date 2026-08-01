using Domain.Interfaces;
using Infrastructure.Repositories.Base;
using RoomDBService.src.Domain.Entities;
using RoomDBService.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RoomImageRepository : RepositoryBase<RoomImage>, IRoomImageRepository
{
    private readonly RoomDbContext _context;
    public RoomImageRepository(RoomDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<RoomImage>> GetByUserId(int roomId)
    {
        return await _context.RoomImages.Where(x => x.RoomId == roomId).ToListAsync();
    }

    public async Task RemoveRange(List<RoomImage> roomImages)
    {
        _context.RoomImages.RemoveRange(roomImages);
        await _context.SaveChangesAsync();
    }
}