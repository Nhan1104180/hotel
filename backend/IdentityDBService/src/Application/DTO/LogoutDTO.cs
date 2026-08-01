namespace Application.DTO;

public class LogoutRequestDTO
{
    public string RefreshToken { get; set; }
}

public class LogoutResponseDTO
{
    public string Message { get; set; }
}