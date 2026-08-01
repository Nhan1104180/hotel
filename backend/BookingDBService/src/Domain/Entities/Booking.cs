using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookingDBService.src.Domain.Entities;

public partial class Booking
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("RoomID")]
    public int RoomId { get; set; }

    [Column("CustomerID")]
    public int CustomerId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CheckInDate { get; set; }

    public DateTime CheckOutDate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalAmount { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = null!;

}
