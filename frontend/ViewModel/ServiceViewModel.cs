using System.ComponentModel.DataAnnotations;

namespace frontend.ViewModel;

public class ServiceViewModel
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string? Name { get; set; } 
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AddServiceViewModel
{
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Không được để trống")]
    [MaxLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Không được để trống")]
    [MaxLength(250, ErrorMessage = "Mô tả không được vượt quá 250 ký tự")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Không được để trống")]
    [Range(1000, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 1000")]
    [RegularExpression(@"^[1-9][0-9]*000$", ErrorMessage = "Giá phải có dạng 1000, 2000, 5000, 100000, 500000... (không có 5125, 123987, 500123, 500001)")]
    public decimal Price { get; set;} 

    public string? ImageUrl { get; set; }
}

public class UpdateServiceViewModel
{
    public int Id { get; set; }
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Không được để trống")]
    [MaxLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Không được để trống")]
    [MaxLength(250, ErrorMessage = "Mô tả không được vượt quá 250 ký tự")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Không được để trống")]
    [Range(1000, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 1000")]
    public decimal Price { get; set;}
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Price % 1000 != 0)
        {
            yield return new ValidationResult(
                "Giá phải là bội số của 1000 (1000, 2000, 5000, 100000...)",
                new[] { nameof(Price) });
        }
    } 
}
