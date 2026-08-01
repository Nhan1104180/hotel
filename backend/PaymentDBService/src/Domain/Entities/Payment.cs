using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Domain.ValueObjects;
using Share.CommonModel;

namespace PaymentDBService.src.Domain.Entities;

public partial class Payment
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("BookingID")]
    public int BookingId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    public int PaymentMethodId { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = null!;

    [StringLength(255)]
    public string? Description { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime PaidAt { get; set; }

    [ForeignKey("PaymentMethodId")]
    [InverseProperty("Payments")]
    public virtual PaymentMethod PaymentMethod { get; set; } = null!;
    public void Complete(PaymentInformation paymentInfo, string description)
    {
        if (Status == PaymentStatus.Paid.ToString())
        {
            throw new InvalidOperationException("Payment đã được thanh toán.");
        }

        Amount = paymentInfo.Amount;
        PaymentMethodId = paymentInfo.PaymentMethodId;
        Description = description;
        PaidAt = DateTime.UtcNow;
        Status = PaymentStatus.Paid.ToString();
    }
}
