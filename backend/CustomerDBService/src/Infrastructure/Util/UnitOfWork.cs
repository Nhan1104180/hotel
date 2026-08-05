using Infrastructure.Data;

namespace Infrastructure.Util;

public class UnitOfWork
{
    public readonly CustomerDbContext _context;

    public UnitOfWork(CustomerDbContext context)
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