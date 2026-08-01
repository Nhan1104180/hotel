using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServiceDBService.src.Domain.Entities;

[Index("Name", Name = "UQ_ServiceCategories_737584F6EA844A4A", IsUnique = true)]
[Index("Name", Name = "UQ__ServiceC__737584F6F67F3EE9", IsUnique = true)]
public partial class ServiceCategory
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(255)]
    public string? ImageUrl { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
