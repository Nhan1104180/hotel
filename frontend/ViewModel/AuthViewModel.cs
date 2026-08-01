using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Không được để trống")]
    [RegularExpression(@"^[a-zA-Z0-9]{6,32}$",ErrorMessage = "Username phải từ 6–32 ký tự và chỉ gồm chữ cái và số")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Không được để trống")]
    [StringLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "Không được để trống")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Không được để trống")]
    [RegularExpression(@"^[A-Z](?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>/?]).{7,}$",
    ErrorMessage = "Mật khẩu phải từ 8 ký tự trở lên, bắt đầu bằng chữ cái in hoa, chứa ít nhất 1 chữ thường, 1 chữ số và 1 ký tự đặc biệt.")]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Không được để trống")]
    [Compare("Password", ErrorMessage = "Mật khẩu không khớp")]
    public string ConfirmPassword { get; set; }
    
    [Required(ErrorMessage = "Không được để trống")]
    [RegularExpression(@"^[0-9]{10,15}$", ErrorMessage = "Số điện thoại phải từ 10–15 ký tự và chỉ gồm số")]
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    public string Phone { get; set; }
    
    [Required(ErrorMessage = "Không được để trống")]
    [StringLength(200, ErrorMessage = "Địa chỉ không được vượt quá 200 ký tự")]
    public string Address { get; set; }
}

public class LoginViewModel
{
    public string EmailOrUsernameOrPhone { get; set; }
    public string Password { get; set; }
}

public class LoginResponse
{
    public string AccessToken { get; set; } = "";
    public string RefreshToken { get; set; } = "";
}

public class RefreshTokenRequestViewModel
{
    public string RefreshToken { get; set; }
}

public class LogoutRequestViewModel
{
    public string RefreshToken { get; set; }
}
