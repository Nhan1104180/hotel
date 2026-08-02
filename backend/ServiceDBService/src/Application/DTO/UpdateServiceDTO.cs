namespace Application.DTO;

public class UpdateServiceDTO
{
    public int CategoryId { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
}