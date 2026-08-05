namespace Domain.ValueObjects;

public class FullName
{
    public string Value { get; private set; }
    public FullName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Full name cannot be empty.");
        }
        if (value.Length < 3 || value.Length > 100)
        {
            throw new ArgumentException("Full name must be between 3 and 100 characters long.");
        }
        Value = value;
    }
}