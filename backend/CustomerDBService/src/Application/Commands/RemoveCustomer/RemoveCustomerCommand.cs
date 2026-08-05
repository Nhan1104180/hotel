using MediatR;
using Share.CommonModel;

namespace Application.Commands.RemoveCustomer;

public class RemoveCustomerCommand : IRequest<ResponseEntity>
{
    public int Id { get; set; }
}