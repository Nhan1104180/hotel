using MediatR;
using Share.CommonModel;

namespace Application.Commands.UpdateService;

public class UpdateServiceCommand : IRequest<ResponseEntity>
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public string Description { get; set; }
}