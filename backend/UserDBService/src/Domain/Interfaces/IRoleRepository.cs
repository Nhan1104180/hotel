using Domain.Entities;
using Domain.Interfaces.Base;

namespace Domain.Interfaces;

public interface IRoleRepository : IRepositoryBase<Role>
{
   Task<List<Role>?> GetByNames(List<string> names);
}