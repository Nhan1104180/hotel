using Domain.Interfaces;
using IdentityDBService.src.Domain.Entities;
using IdentityDBService.src.Infrastructure.Data;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    private readonly IdentityDbContext _context;
    public UserRepository(IdentityDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task AddAsync(UserRole roleID)
    {
        await _context.UserRoles.AddAsync(roleID);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<bool> ExistsByPhoneNumberAsync(string phone)
    {
        return await _context.Users.AnyAsync(u => u.Phone == phone);
    }

    public async Task<bool> ExistsByUsernameAsync(string username)
    {
        return await _context.Users.AnyAsync(u => u.Username == username);
    }

    public async Task<User?> FindByIdentifierAsync(string identifier)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == identifier || u.Email == identifier || u.Phone == identifier);
    }

}
