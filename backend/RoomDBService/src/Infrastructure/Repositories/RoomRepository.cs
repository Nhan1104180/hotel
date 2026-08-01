using Domain.Interfaces;
using Infrastructure.Repositories.Base;
using RoomDBService.src.Domain.Entities;
using RoomDBService.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Application.DTO;
using Domain.Enums;

namespace Infrastructure.Repositories;

public class RoomRepository : RepositoryBase<Room>, IRoomRepository
{
    private readonly RoomDbContext _context;
    public RoomRepository(RoomDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<RoomDTO>> GetAllRoomsAsync()
    {
        // var query = _context.Rooms
        //     .Include(x => x.RoomType)
        //     .OrderBy(x => x.RoomNumber);

        // var items = await query
        //     .Select(r => new RoomDTO
        //     {
        //         Id = r.Id,
        //         RoomNumber = r.RoomNumber,
        //         RoomTypeId = r.RoomTypeId,
        //         Price = r.Price,
        //         Status = r.Status,
        //         RoomType = r.RoomType.Name,
        //         Description = r.RoomType.Description,
        //         Capacity = r.RoomType.Capacity
        //     })
        //     .ToListAsync();
        return await _context.Rooms
            .Include(x => x.RoomType)
            .OrderBy(x => x.RoomNumber)
            .Select(r => new RoomDTO
            {
                Id = r.Id,
                RoomNumber = r.RoomNumber,
                RoomTypeId = r.RoomTypeId,
                Price = r.Price,
                Status = r.Status,
                RoomType = r.RoomType.Name,
                Description = r.RoomType.Description,
                Capacity = r.RoomType.Capacity
            }).ToListAsync();

    }

    public async Task<RoomDTO> GetRoomByIdAsync(int id)
    {
        return await _context.Rooms
            .Include(x => x.RoomType)
            .Where(x => x.Id == id)
            .Select(r => new RoomDTO
            {
                Id = r.Id,
                RoomNumber = r.RoomNumber,
                Price = r.Price,
                Status = r.Status,
                RoomType = r.RoomType.Name,
                Description = r.RoomType.Description,
                Capacity = r.RoomType.Capacity

            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ExistsByRoomNumberAsync(string roomNumber)
    {
        return await _context.Rooms.AnyAsync(x => x.RoomNumber == roomNumber);
    }

    public async Task<Room> AddRoomAsync(Room room)
    {
        await _context.Rooms.AddAsync(room);
        await _context.SaveChangesAsync();
        return room;
    }

    public async Task RemoveRange(List<RoomImage> roomImages)
    {
        _context.RoomImages.RemoveRange(roomImages);
        await _context.SaveChangesAsync();
    }

    public async Task<List<RoomDTO>> SearchRoomAsync(string keyword, int pageIndex, int pageSize)
    {
        return await _context.Rooms
            .Include(x => x.RoomType)
            .Where(x => x.RoomNumber.Contains(keyword) || x.RoomType.Name.Contains(keyword))
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new RoomDTO
            {
                Id = r.Id,
                RoomNumber = r.RoomNumber,
                RoomTypeId = r.RoomTypeId,
                Price = r.Price,
                Status = r.Status,
                RoomType = r.RoomType.Name,
                Description = r.RoomType.Description,
                Capacity = r.RoomType.Capacity
            })
            .ToListAsync();
    }

    public async Task<List<AvailableRoomDTO>> GetAvailableRoomsAsync(int pageIndex, int pageSize)
    {
        return await _context.Rooms
             .Include(x => x.RoomType)
             .Where(x => x.Status == RoomStatus.Available.ToString())
             .Skip((pageIndex - 1) * pageSize)
             .Take(pageSize)
             .Select(r => new AvailableRoomDTO
             {
                 Id = r.Id,
                 Number = r.RoomNumber,
                 RoomType = r.RoomType.Name,
                 Price = r.Price,
                 Capacity = r.RoomType.Capacity
             }).ToListAsync();
    }

    public async Task<Room?> GetRoomEntityByIdAsync(int id)
    {
        return await _context.Rooms.FindAsync(id);
    }

}