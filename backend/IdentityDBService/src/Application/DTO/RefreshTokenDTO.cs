namespace Application.DTO;

public class RefreshTokenRequestDTO
{
    public string RefreshToken { get; set; }
}

public class RefreshTokenResponseDto
{
    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }
}
