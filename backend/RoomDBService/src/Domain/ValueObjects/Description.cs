namespace Domain.ValueObjects;

public class Description
{
    public string Value { get; private set; }
    public Description(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Description cannot be empty", nameof(value));
        }
        Value = value;
    }
}