namespace Application.DTO;

public class AvailableRoomDTO
{
    public int Id { get; set; }
    public string Number { get; set; }
    public string RoomType { get; set; }
    public decimal Price { get; set; }
    public int Capacity { get; set; }
}