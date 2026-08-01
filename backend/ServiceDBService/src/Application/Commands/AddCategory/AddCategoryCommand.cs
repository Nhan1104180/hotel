using MediatR;
using ServiceDBService.src.Domain.Entities;
using Share.CommonModel;

namespace Application.Commands.AddCategory;

public class AddCategoryCommand : IRequest<ResponseEntity>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
}