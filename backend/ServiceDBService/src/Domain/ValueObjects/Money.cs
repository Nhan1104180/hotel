namespace Domain.ValueObjects;

public class Money
{
   public decimal Value { get; }

    public Money(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("Price must be greater than or equal to 0.");

        Value = value;
    }
}