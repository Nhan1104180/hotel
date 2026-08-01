namespace Domain.ValueObjects;

public class Name
{
    public string Value { get; private set; }
    public Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Name cannot be empty", nameof(value));
        }
        Value = value;
    }
}