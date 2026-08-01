using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentityDBService.src.Domain.Entities;
using IdentityDBService.src.Infrastructure.Data;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public interface IJwtService
{
    // string GenerateToken(User userLogin);
    string DecodePayloadToken(string token);
    string GenerateToken(User userLogin);
    string GenerateRefreshToken();
}

public class JwtService : IJwtService
{
    private readonly string? _key;
    private readonly string? _issuer;
    private readonly string? _audience;
    private readonly IdentityDbContext _context;
    public JwtService(IConfiguration Configuration, IdentityDbContext db)
    {
        // Iconfiguration Configuration dùng để lấy cấu hình từ appsetting.json
        _key = Configuration["Jwt:Secret-Key"]; // lấy từ appsetting.json
        _issuer = Configuration["Jwt:Issuer"];
        _audience = Configuration["Jwt:Audience"];
        _context = db;
    }

    public string GenerateToken(User userLogin)
    {
        // Khóa bí mật để ký token
        var key = Encoding.ASCII.GetBytes(_key);
        // Tạo danh sách các claims cho token
        var claims = new List<Claim>
        {
            // ID
            new Claim("id", userLogin.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, userLogin.Id.ToString()),


            // Claim mặc định cho Role
            new Claim(ClaimTypes.Name, userLogin.Username),
            new Claim(ClaimTypes.Email, userLogin.Email),
            
            // user nam , email , role
            new Claim("Username", userLogin.Username),
            new Claim("Email", userLogin.Email),
            new Claim(JwtRegisteredClaimNames.Sub, userLogin.Username),   // Subject của token
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique ID của token
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()) // Thời gian tạo token
        };
        // lay role cua user UserRole  , lay ra nhung dòng có userid= userlogin.id
        var userRoles = _context.UserRoles
            .Where(ur => ur.UserId == userLogin.Id) // lọc theo UserId
            .Select(ur => ur.Role.Name)// lấy RoleName từ bảng Role
            .ToList();
        // Thêm claims cho từng role của user
        foreach (var role in userRoles)// duyệt qua từng role
        {
            claims.Add(new Claim(ClaimTypes.Role, role)); // thêm claim Role
        }


        // Tạo khóa bí mật để ký token
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature
        );
        // Thiết lập thông tin cho token
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(30), // Token hết hạn sau 1 giờ
            SigningCredentials = credentials,
            Issuer = _issuer,                 // Thêm Issuer vào token
            Audience = _audience,              // Thêm Audience vào token
        };
        // Tạo token bằng JwtSecurityTokenHandler
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        // Trả về chuỗi token đã mã hóa
        return tokenHandler.WriteToken(token);
    }

    public string DecodePayloadToken(string token)
    {
        try
        {
            // Kiểm tra token có null hoặc rỗng không
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token không được để trống", nameof(token));
            }

            // Tạo handler và đọc token
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Lấy username từ claims (thường nằm trong claim "sub" hoặc "name")
            var usernameClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "Username"); // Common in some identity providers

            if (usernameClaim == null)
            {
                throw new InvalidOperationException("Không tìm thấy username trong payload");
            }

            return usernameClaim.Value;
        }
        catch (Exception ex)
        {
            // Xử lý lỗi (có thể log lỗi ở đây)
            throw new InvalidOperationException($"Lỗi khi decode token: {ex.Message}", ex);
        }
    }

    public string GenerateRefreshToken()
    {
        return Guid.NewGuid()
            .ToString()
            .Replace("-", "");
    }
}