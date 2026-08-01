using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[PrimaryKey("UserId", "RoleId")]
public partial class UserRole
{
    [Key]
    [Column("UserID")]
    public int UserId { get; set; }

    [Key]
    [Column("RoleID")]
    public int RoleId { get; set; }

    public DateTime CreateAt { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("UserRoles")]
    public virtual Role Role { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("UserRoles")]
    public virtual User User { get; set; } = null!;

}
