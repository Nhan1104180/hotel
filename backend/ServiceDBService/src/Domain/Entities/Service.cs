using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServiceDBService.src.Domain.Entities;

[Index("Name", Name = "UQ_Services_737584F6EA844A4A", IsUnique = true)]
[Index("Name", Name = "UQ__Services__737584F6561513FF", IsUnique = true)]
public partial class Service
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("CategoryID")]
    public int? CategoryId { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    [StringLength(255)]
    public string? ImageUrl { get; set; }

    public string Status { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Services")]
    public virtual ServiceCategory? Category { get; set; }

    [InverseProperty("Service")]
    public virtual ICollection<ServiceUsage> ServiceUsages { get; set; } = new List<ServiceUsage>();
}
