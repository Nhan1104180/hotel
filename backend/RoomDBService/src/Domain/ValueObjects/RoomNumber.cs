using System.Text.RegularExpressions;

namespace Domain.ValueObjects;

public class RoomNumber
{
    public string Value { get; private set; }

    public RoomNumber(char prefix, int roomNo)
    {
        if (!char.IsLetter(prefix) || !char.IsUpper(prefix))
        {
            throw new ArgumentException("Prefix must be an uppercase letter (A-Z).");
        }

        if (roomNo < 1 || roomNo > 999)
        {
            throw new ArgumentException("Room number must be between 1 and 999.");
        }

        Value = $"{prefix}{roomNo:D3}"; // 1 -> 001, 12 -> 012, 101 -> 101
    }

    public RoomNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Room number cannot be empty.", nameof(value));
        }

        if (!Regex.IsMatch(value, @"^[A-Z][0-9]{3}$"))
        {
            throw new ArgumentException("Room number must be in format A001, A012, A101...");
        }

        Value = value;
    }

    public override string ToString() => Value;
}