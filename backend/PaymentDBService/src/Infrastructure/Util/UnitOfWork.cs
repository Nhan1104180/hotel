using PaymentDBService.src.Infrastructure.Data;

namespace Infrastructure.Util;

public class UnitOfWork
{
    public readonly PaymentDbContext _context;

    public UnitOfWork(PaymentDbContext context)
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