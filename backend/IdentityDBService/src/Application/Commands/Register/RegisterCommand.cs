using Application.DTO;
using MediatR;
using Share.CommonModel;

namespace Application.Commands.Register;

public class RegisterCommand : IRequest<ResponseEntity>
{
    public string Username { get; set; } 
     public string FullName { get; set; } 
    public string Email { get; set; } 
    public string Password { get; set; } 
    public string Phone { get; set; } 
    public string Address { get; set; } 
}