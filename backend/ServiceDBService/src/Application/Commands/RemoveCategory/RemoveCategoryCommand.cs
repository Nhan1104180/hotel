using MediatR;
using Share.CommonModel;

namespace Application.Commands.RemoveCategory;

public class RemoveCategoryCommand : IRequest<ResponseEntity>
{
    public int Id { get; set; }
}