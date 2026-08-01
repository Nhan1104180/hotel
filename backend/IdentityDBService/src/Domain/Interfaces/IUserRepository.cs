using IdentityDBService.src.Domain.Entities;

namespace Domain.Interfaces;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<bool> ExistsByEmailAsync(string email);
    Task<bool> ExistsByUsernameAsync(string username);
    Task<bool> ExistsByPhoneNumberAsync(string phoneNumber);
    Task<User?> FindByIdentifierAsync(string identifier);
    Task AddAsync(UserRole roleID);
}