using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRoleRepository : RepositoryBase<UserRole>, IUserRoleRepository
{
    private readonly UserDbContext _context;
    public UserRoleRepository(UserDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task AddRangeRole(List<UserRole> userRoles)
    {
        await _context.UserRoles.AddRangeAsync(userRoles);
        await _context.SaveChangesAsync();
    }

    public async Task<List<UserRole>> GetByUserId(int userId)
    {
        return await _context.UserRoles.Where(x => x.UserId == userId).ToListAsync();

    }

    public async Task RemoveRange(List<UserRole> userRoles)
    {
        _context.UserRoles.RemoveRange(userRoles);
        await _context.SaveChangesAsync();
    }
}