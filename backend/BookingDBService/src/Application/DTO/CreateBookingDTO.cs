namespace Application.DTO;

public class CreateBookingDTO
{
    public int RoomId { get; set; }
    public int CustomerId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
}