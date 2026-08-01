namespace Domain.ValueObjects;

public class CheckoutTime
{
    public DateTime Value { get; private set; }

    public CheckoutTime(DateTime value)
    {
        Value = value;
    }
}