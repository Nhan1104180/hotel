using MediatR;
using Share.CommonModel;

namespace UserDBService.Application.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<ResponseEntity>
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
}