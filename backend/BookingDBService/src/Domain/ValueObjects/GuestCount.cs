namespace Domain.ValueObjects;

public class GuestCount
{
    public int Value { get; private set; }

    public GuestCount(int value)
    {
        if (value <= 0)
        {
            throw new ArgumentException("Số lượng khách phải lớn hơn 0");
        }
        Value = value;
    }
}