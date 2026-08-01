using Domain.Interfaces;
using Infrastructure.Repositories.Base;
using RoomDBService.src.Domain.Entities;
using RoomDBService.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RoomTypeRepository : RepositoryBase<RoomType>, IRoomTypeRepository
{
    private readonly RoomDbContext _context;
    public RoomTypeRepository(RoomDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<RoomType>> GetAllRoomTypeAsync()
    {
        return await _context.RoomTypes.ToListAsync();
    }

    public async Task<RoomType> GetRoomTypeAsync(int roomTypeId)
    {
        return await _context.RoomTypes.FirstOrDefaultAsync(x => x.Id == roomTypeId);
    }

    public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
    {
        //return await _context.RoomTypes.AnyAsync(x => x.Name == name);
        var query = _context.RoomTypes.Where(x => x.Name == name);

        if (excludeId.HasValue)
        {
            query = query.Where(x => x.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<RoomType> AddRoomTypeAsync(RoomType roomType)
    {
        await _context.RoomTypes.AddAsync(roomType);
        await _context.SaveChangesAsync();
        return roomType;
    }

    public async Task<RoomType> UpdateRoomTypeAsync(RoomType roomType)
    {
        _context.RoomTypes.Update(roomType);
        await _context.SaveChangesAsync();
        return roomType;
    }

}