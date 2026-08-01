using Domain.Interfaces;
using Infrastructure.Repositories.Base;
using ServiceDBService.src.Domain.Entities;
using ServiceDBService.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Application.DTO;

namespace Infrastructure.Repositories;

public class ServiceCategoryRepository : RepositoryBase<ServiceCategory>, IServiceCategoryRepository
{
    private readonly ServiceDbContext _context;
    public ServiceCategoryRepository(ServiceDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<CategoryDTO>> GetAllCategoryAsync()
    {
        return await _context.ServiceCategories.Select(x => new CategoryDTO
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description
        }).ToListAsync();
    }

    public Task<ServiceCategory> GetCategoryAsync(int categoryId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await _context.ServiceCategories.AnyAsync(c => c.Name == name);
    }

    public async Task<ServiceCategory> AddCategoryAsync(ServiceCategory category)
    {
        await _context.ServiceCategories.AddAsync(category);
        return category;
    }

}