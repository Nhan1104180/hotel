namespace Application.DTO;

public class CreatePaymentDTO
{
    public int BookingId { get; set; }
    public decimal Amount { get; set; }
    public int PaymentMethodId { get; set; }
    public string? Description { get; set; }
}