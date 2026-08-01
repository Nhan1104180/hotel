namespace Application.DTO;

public class ServiceUsageDTO
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public int ServiceId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
}
