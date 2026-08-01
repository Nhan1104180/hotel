using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RoleRepository : RepositoryBase<Role>, IRoleRepository
{
   private readonly UserDbContext _context;
   public RoleRepository(UserDbContext context) : base(context)
   {
      _context = context;
   }

   public async Task<List<Role>?> GetByNames(List<string> names)
   {
      return await _context.Roles.Where(x => names.Contains(x.Name)).ToListAsync();
   }
   
}