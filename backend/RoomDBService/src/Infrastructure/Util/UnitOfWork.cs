using RoomDBService.src.Infrastructure.Data;

namespace Infrastructure.Util;

public class UnitOfWork
{
    public readonly RoomDbContext _context;

    public UnitOfWork(RoomDbContext context)
    {
        _context = context;
    }


    public async Task BeginTransactionAsync()
    {
        await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransaction()
    {
        await _context.Database.CommitTransactionAsync();
    }

    public async Task RollbackTransaction()
    {
        await _context.Database.RollbackTransactionAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }


}