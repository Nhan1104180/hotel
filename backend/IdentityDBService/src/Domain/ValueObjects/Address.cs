namespace Domain.ValueObjects;

public class Address
{
    public string Value { get; private set; }

    public Address(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new Exception("Address không được để trống");
        }

        Value = value;
    }
}