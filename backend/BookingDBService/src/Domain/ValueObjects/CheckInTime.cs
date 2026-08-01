namespace Domain.ValueObjects;

public class CheckInTime
{
    public DateTime Value { get; private set; }

    public CheckInTime(DateTime value)
    {
        Value = value;
    }
}