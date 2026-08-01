using Domain.Interfaces;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using ServiceDBService.src.Domain.Entities;
using ServiceDBService.src.Infrastructure.Data;

namespace Infrastructure.Repositories;

public class ServiceUsageRepository : RepositoryBase<ServiceUsage>, IServiceUsageRepository
{
    private readonly ServiceDbContext _context;
    public ServiceUsageRepository(ServiceDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<ServiceUsage>> GetServiceUsageByBookingIdAsync(int bookingId)
    {
        return await _context.ServiceUsages
            .Include(x => x.Service)
            .Where(x => x.BookingId == bookingId)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(int bookingId, int serviceId)
    {
        return await _context.ServiceUsages.AnyAsync(x => x.BookingId == bookingId && x.ServiceId == serviceId);
    }

    public async Task<ServiceUsage> AddServiceUsageAsync(ServiceUsage serviceUsage)
    {
        await _context.ServiceUsages.AddAsync(serviceUsage);
        return serviceUsage;
    }

    public async Task<ServiceUsage> GetAsync(int bookingId, int serviceId)
    {
        return await _context.ServiceUsages.Include(x => x.Service).FirstOrDefaultAsync(x => x.BookingId == bookingId && x.ServiceId == serviceId);
    }

    public async Task<ServiceUsage> DeleteServiceUsageAsync(ServiceUsage serviceUsage)
    {
        _context.ServiceUsages.Remove(serviceUsage);
        return serviceUsage;
    }
}