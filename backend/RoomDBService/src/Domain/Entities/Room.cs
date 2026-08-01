using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace RoomDBService.src.Domain.Entities;

[Index("RoomNumber", Name = "UQ_Rooms_737584F6EA844A4A", IsUnique = true)]
public partial class Room
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(10)]
    public string? RoomNumber { get; set; }

    [Column("RoomTypeID")]
    public int RoomTypeId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = null!;

    [InverseProperty("Room")]
    public virtual ICollection<RoomImage> RoomImages { get; set; } = new List<RoomImage>();

    [ForeignKey("RoomTypeId")]
    [InverseProperty("Rooms")]
    public virtual RoomType RoomType { get; set; } = null!;
}
