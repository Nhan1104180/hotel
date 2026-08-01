namespace Domain.ValueObjects;

public class Quantity
{
    public int Value { get; set; }
    public Quantity(int value)
    {
        if (value < 0)
        {
            throw new ArgumentException("Số lượng phải lớn hơn 0");
        }
        Value = value;
    }
}