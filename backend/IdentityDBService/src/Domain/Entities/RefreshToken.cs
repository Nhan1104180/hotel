using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IdentityDBService.src.Domain.Entities;

public partial class RefreshToken
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public bool IsRevoked { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? RevokedAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("RefreshTokens")]
    public virtual User User { get; set; } = null!;

    public RefreshToken()
    {
        
    }

    public RefreshToken(int userId, string token, DateTime expiresAt, bool isRevoked, DateTime createAt, DateTime? revokedAt)
    {
        UserId = userId;
        Token = token;
        ExpiresAt = expiresAt;
        IsRevoked = isRevoked;
        CreateAt = createAt;
        RevokedAt = revokedAt;
    }

    public bool IsExpired()
    {
        return DateTime.UtcNow > ExpiresAt;
    }

    public void Revoke()
    {
        IsRevoked = true;
        RevokedAt = DateTime.UtcNow;
    }
}
