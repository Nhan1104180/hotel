using MediatR;
using Share.CommonModel;

namespace Application.Commands.UpdateCategory;

public class UpdateCategoryCommand : IRequest<ResponseEntity>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; }
}