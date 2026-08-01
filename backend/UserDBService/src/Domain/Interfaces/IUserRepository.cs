using Domain.Entities;
using Domain.Interfaces.Base;

namespace Domain.Interfaces;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<List<User>> GetAllUsers(int pageNumber, int pageSize);
    Task<int> CountAsync();
    Task<User?> GetUserById(int id);
    Task<bool> ExistsByUsernameAsync(string username);
    Task<bool> ExistsByEmailAsync(string email);
    Task<bool> ExistsByPhoneAsync(string phone);
    Task<User> CreateUser(User user);
    Task<bool> ExistsByUsernameAsync(string username,int excludeId);
    Task<bool>ExistsByEmailAsync(string email,int excludeId);
    Task<bool>ExistsByPhoneAsync(string phone,int excludeId);
    
}