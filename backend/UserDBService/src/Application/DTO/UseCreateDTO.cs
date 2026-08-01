namespace Application.DTO;

public class UserCreateDTO
{
    public string Username { get; set; } 
    public string FullName { get; set; } 
    public string Email { get; set; }
    public string Phone { get; set; } 
    public string Address { get; set; }
    public List<string> RoleNames { get; set; } =  new List<string>();
}