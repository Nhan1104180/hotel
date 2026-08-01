using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Share.CommonModel;

namespace Infrastructure.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    private readonly UserDbContext _context;
    public UserRepository(UserDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllUsers(int pageNumber, int pageSize)
    {
        return await _context.Users
            .OrderByDescending(x => x.CreateAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> CountAsync()
    {
        return await _context.Users.CountAsync();
    }

    public async Task<User?> GetUserById(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
    }

    //CreateUser
    public async Task<bool> ExistsByUsernameAsync(string username)
    {
        return await _context.Users.AnyAsync(x => x.Username == username);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Users.AnyAsync(x => x.Email == email);
    }

    public async Task<bool> ExistsByPhoneAsync(string phone)
    {
        return await _context.Users.AnyAsync(x => x.Phone == phone);
    }

    public async Task<User> CreateUser(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    //UpdateUser
    public async Task<bool> ExistsByUsernameAsync(string username, int excludeId)
    {
        return await _context.Users.AnyAsync(x => x.Username == username && x.Id != excludeId);
    }

    public async Task<bool> ExistsByEmailAsync(string email, int excludeId)
    {
        return await _context.Users.AnyAsync(x => x.Email == email && x.Id != excludeId);
    }

    public async Task<bool> ExistsByPhoneAsync(string phone, int excludeId)
    {
        return await _context.Users.AnyAsync(x => x.Phone == phone && x.Id != excludeId);
    }
}