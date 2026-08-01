using Application.DTO;
using ServiceDBService.src.Domain.Entities;

namespace Domain.Interfaces;

public interface IServiceRepository : IRepositoryBase<Service>
{
    Task<List<ServiceDTO>> GetAllServicesAsync();
    Task<ServiceDTO> GetServiceByIdAsync(int id);
    Task<bool> ExistsByNameAsync(string name);
    Task<Service> AddServiceAsync(Service service);
    Task<List<ServiceDTO>> SearchServiceAsync(string keyword, int pageIndex, int pageSize);
}