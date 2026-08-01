using ServiceDBService.src.Domain.Entities;

namespace Domain.Interfaces;

public interface IServiceUsageRepository
{
    Task<List<ServiceUsage>> GetServiceUsageByBookingIdAsync(int bookingId);
    Task<bool> ExistsAsync(int bookingId, int serviceId);
    Task<ServiceUsage> AddServiceUsageAsync(ServiceUsage serviceUsage);
    Task<ServiceUsage> GetAsync(int bookingId, int serviceId);
    Task<ServiceUsage> DeleteServiceUsageAsync(ServiceUsage serviceUsage);
}