namespace Domain.ValueObjects;

public class RefreshTokenValue
{
    public string Value { get; }

    public RefreshTokenValue(string value)
    {
        if(string.IsNullOrWhiteSpace(value))
        {
            throw new Exception("Refresh token required");
        }

        if(value.Length < 20)
        {
            throw new Exception("Invalid refresh token");
        }

        Value = value;
    }
}