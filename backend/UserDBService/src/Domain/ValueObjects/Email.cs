namespace Domain.ValueObjects;

public class Email 
{
    public string Value { get; set; }
    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email cannot be empty.");
        }
        if (!value.Contains("@"))
        {
            throw new ArgumentException("Invalid email format.");
        }
        Value = value.Trim().ToLower();
    }
}