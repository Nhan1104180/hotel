namespace Domain.ValueObjects;

public class Money
{
    public decimal Amount { get; private set; }
    public Money(decimal amount)
    {
        if (amount <= 1000)
        {
            throw new ArgumentException("Amount cannot be negative", nameof(amount));
        }
        Amount = amount;
    }
}