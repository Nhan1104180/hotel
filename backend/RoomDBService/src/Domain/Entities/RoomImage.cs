using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RoomDBService.src.Domain.Entities;

public partial class RoomImage
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("RoomID")]
    public int RoomId { get; set; }

    [Column("ImageURL")]
    [StringLength(255)]
    public string ImageUrl { get; set; } = null!;

    [ForeignKey("RoomId")]
    [InverseProperty("RoomImages")]
    public virtual Room Room { get; set; } = null!;
}
