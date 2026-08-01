namespace Domain.ValueObjects;

public class ServiceCategoryName
{
    public string Value;

    public ServiceCategoryName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Tên danh mục dịch vụ không được để trống.");
        }

        if (value.Length > 100)
        {
            throw new ArgumentException("Tên danh mục dịch vụ không được vượt quá 100 ký tự.");
        }

        Value = value;
    }

   
}