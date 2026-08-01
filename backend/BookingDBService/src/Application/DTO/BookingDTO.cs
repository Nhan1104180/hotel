namespace Application.DTO;

public class BookingDTO
{
    public int UserId { get; set; }
    public int RoomNumber { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int TotalAmount { get; set; }
    public string Status { get; set; }
}