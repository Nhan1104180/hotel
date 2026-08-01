using MediatR;
using Share.CommonModel;

namespace Application.Commands.AddUser;

public class AddUserCommand : IRequest<ResponseEntity>
{
    public string Username { get; set; }
    public string FullName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public List<string> RoleNames { get; set; }
}