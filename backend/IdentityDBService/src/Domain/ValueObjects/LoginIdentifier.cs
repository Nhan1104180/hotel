namespace Domain.ValueObjects;

public class LoginIdentifier
{
    public string Value { get; private set; }
    public LoginIdentifier(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new Exception("Username/Email/Phone required");
        }
        Value = value.Trim();
    }
}