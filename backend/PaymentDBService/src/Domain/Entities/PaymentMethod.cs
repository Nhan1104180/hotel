using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PaymentDBService.src.Domain.Entities;

[Index("Name", Name = "UQ_PaymentMethods_737584F6EA844A4A", IsUnique = true)]
[Index("Name", Name = "UQ__PaymentM__737584F68EF9A8A8", IsUnique = true)]
public partial class PaymentMethod
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [InverseProperty("PaymentMethod")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
