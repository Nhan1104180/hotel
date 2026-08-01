namespace Domain.ValueObjects;

public class PaymentInformation
{
    public decimal Amount { get; set; }
    public int PaymentMethodId { get; set; }
    public PaymentInformation(decimal amount, int paymentMethodId)
    {
        Amount = amount;
        PaymentMethodId = paymentMethodId;
    }

   
}