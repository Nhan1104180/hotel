namespace Domain.ValueObjects;

public class UserName
{
    public string Value { get; private set; }
    public UserName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Username cannot be empty", nameof(value));
        }
        Value = value;
    }
}