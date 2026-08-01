namespace Domain.ValueObjects;

public class DateRange
{
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }

    public DateRange(DateTime checkIn, DateTime checkOut)
    {
        if (checkIn > checkOut)
        {
            throw new ArgumentException("Check-out phải nhỏ hơn Check-in");
        }
        CheckIn = checkIn;
        CheckOut = checkOut;
    }

}