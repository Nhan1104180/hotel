namespace Application.DTO;

public class ServiceDTO
{
    public int Id { get; set; }
    public int? CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
