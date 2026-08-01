namespace Domain.ValueObjects;

public class Money
{
    public decimal Value { get; private set; }
    
    public Money(decimal value)
    {
        if(value < 0){
            throw new ArgumentException("Amount must be non-negative");
        }
        Value = value;
    }
}