using Application.DTO;
using ServiceDBService.src.Domain.Entities;

namespace Domain.Interfaces;

public interface IServiceCategoryRepository : IRepositoryBase<ServiceCategory>
{
    Task<List<CategoryDTO>> GetAllCategoryAsync();
    Task<bool> ExistsByNameAsync(string name);
    Task<ServiceCategory> AddCategoryAsync(ServiceCategory category);
}