namespace Domain.ValueObjects;

public class ServiceName 
{
    public string Value { get; }

    public ServiceName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Service name is required.");

        if (value.Length > 100)
            throw new ArgumentException("Service name cannot exceed 100 characters.");

        Value = value.Trim();
    }

}