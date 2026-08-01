using Domain.Entities;
using Domain.Interfaces.Base;

namespace Domain.Interfaces;

public interface IUserRoleRepository : IRepositoryBase<UserRole>
{
    Task<List<UserRole>>GetByUserId(int userId);
    Task AddRangeRole(List<UserRole> userRoles);
    Task RemoveRange(List<UserRole> userRoles);
}