using MediatR;
using Share.CommonModel;

namespace Application.Commands.AddService;

public class AddServiceCommand : IRequest<ResponseEntity>
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
}