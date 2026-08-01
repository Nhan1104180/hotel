namespace Domain.ValueObjects;

public class Capacity
{
    public int Value { get; private set; }
    public Capacity(int value)
    {
        if (value <= 0)
        {
            throw new ArgumentException("Room capacity must be greater than 0", nameof(value));
        }
        Value = value;
    }
}