using Domain.Interfaces;
using IdentityDBService.src.Domain.Entities;
using IdentityDBService.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IdentityDBService.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly IdentityDbContext _context;

    public RefreshTokenRepository(IdentityDbContext context)
    {
        _context = context;
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await _context.RefreshTokens.FirstOrDefaultAsync(x =>x.Token == token);
    }

    public async Task AddAsync(RefreshToken refreshToken)
    {
        await _context.RefreshTokens .AddAsync(refreshToken);
    }

    public async Task UpdateAsync(RefreshToken refreshToken)
    {
         _context.RefreshTokens.Update(refreshToken);
        await _context.SaveChangesAsync();
    }
}