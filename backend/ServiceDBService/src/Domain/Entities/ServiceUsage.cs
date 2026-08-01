using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServiceDBService.src.Domain.Entities;

public partial class ServiceUsage
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("BookingID")]
    public int BookingId { get; set; }

    [Column("ServiceID")]
    public int ServiceId { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal UnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalAmount { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey("ServiceId")]
    [InverseProperty("ServiceUsages")]
    public virtual Service Service { get; set; } = null!;
}
