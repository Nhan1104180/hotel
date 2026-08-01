namespace Domain.ValueObjects;

public class Password
{
    public string HashedValue { get; private set; }
    public Password(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Password cannot be empty.");
        }
        if (value.Length < 6)
        {
            throw new ArgumentException("Password must be at least 6 characters long.");
        }
        HashedValue = BCrypt.Net.BCrypt.HashPassword(value);
    }  
}