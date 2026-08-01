using Application.DTO;
using Domain.Interfaces;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using ServiceDBService.src.Domain.Entities;
using ServiceDBService.src.Infrastructure.Data;

namespace Infrastructure.Repositories;

public class ServiceRepository : RepositoryBase<Service>, IServiceRepository
{
    private readonly ServiceDbContext _context;
    public ServiceRepository(ServiceDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<ServiceDTO>> GetAllServicesAsync()
    {
        return await _context.Services
            .Include(x => x.Category)
            .OrderBy(x => x.Name)
            .Select(r => new ServiceDTO
            {
                Id = r.Id,
                CategoryId = r.CategoryId,
                Name = r.Name,
                Price = r.Price,
                Description = r.Description,
                Status = r.Status,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<ServiceDTO> GetServiceByIdAsync(int id)
    {
        return await _context.Services
            .Include(x => x.Category)
            .Where(x => x.Id == id)
            .Select(r => new ServiceDTO
            {
                Id = r.Id,
                CategoryId = r.CategoryId,
                Name = r.Name,
                Price = r.Price,
                Description = r.Description,
                Status = r.Status,
                CreatedAt = r.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await _context.Services.AnyAsync(x => x.Name == name);
    }

    public async Task<Service> AddServiceAsync(Service service)
    {
        await _context.Services.AddAsync(service);
        return service;
    }

    public async Task<List<ServiceDTO>> SearchServiceAsync(string keyword, int pageIndex, int pageSize)
    {
        return await _context.Services
            .Include(x => x.Category)
            .Where(x => EF.Functions.Collate(x.Name, "Latin1_General_100_CI_AI")
                            .Contains(keyword))
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new ServiceDTO
            {
                Id = r.Id,
                CategoryId = r.CategoryId,
                Name = r.Name,
                Price = r.Price,
                Description = r.Description,
                Status = r.Status,
                CreatedAt = r.CreatedAt
            }).ToListAsync();
    }


}