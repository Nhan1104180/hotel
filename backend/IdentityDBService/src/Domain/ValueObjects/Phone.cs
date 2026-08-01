namespace Domain.ValueObjects;

public class Phone
{
    public string Value { get; private set; }
    public Phone(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Phone number cannot be empty.");
        }
        if (value.Length < 9 || value.Length > 15)
        {
            throw new ArgumentException("Phone number must be between 9 and 15 digits long.");
        }
    
        Value = value;
    }
}