using MediatR;
using Share.CommonModel;

namespace Application.Commands.RemoveService;

public class RemoveServiceCommand : IRequest<ResponseEntity>
{
    public int Id { get; set; }
}