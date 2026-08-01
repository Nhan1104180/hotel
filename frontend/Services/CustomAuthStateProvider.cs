using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();
    private IConfiguration _config;

    public CustomAuthStateProvider(ILocalStorageService localStorage, IConfiguration Configuration)
    {
        _localStorage = localStorage;
        _config = Configuration;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            // Lấy token từ LocalStorage
            var token = await _localStorage.GetItemAsync<string>("AccessToken");

            // Không có token => Chưa đăng nhập
            if (string.IsNullOrWhiteSpace(token))
            {
                return new AuthenticationState(
                    new ClaimsPrincipal(new ClaimsIdentity()));
            }

            token = token.Trim('"');

            // Cấu hình kiểm tra JWT
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_config["Jwt:Secret-Key"])),

                ValidateIssuer = true,
                ValidIssuer = _config["Jwt:Issuer"],

                ValidateAudience = true,
                ValidAudience = _config["Jwt:Audience"],

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,

                RoleClaimType = ClaimTypes.Role,
                NameClaimType = ClaimTypes.Name
            };

            // Validate token
            var principal = new JwtSecurityTokenHandler()
                .ValidateToken(token, tokenValidationParameters, out _);

            var identity = new ClaimsIdentity(
                principal.Claims,
                "jwt",
                ClaimTypes.Name,
                ClaimTypes.Role);

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        catch (InvalidOperationException)
        {
            // Blazor đang prerender, JSInterop (LocalStorage) chưa sẵn sàng
            return new AuthenticationState(
                new ClaimsPrincipal(new ClaimsIdentity()));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            try
            {
                // Token không hợp lệ => xóa khỏi LocalStorage
                await _localStorage.RemoveItemAsync("AccessToken");
            }
            catch
            {
                // JS chưa sẵn sàng thì bỏ qua
            }

            return new AuthenticationState(
                new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    public void NotifyUserAuthentication(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token.Trim('"'));

        var identity = new ClaimsIdentity(jwtToken.Claims, "jwt", ClaimTypes.Name, ClaimTypes.Role);
        var user = new ClaimsPrincipal(identity);

        var authState = Task.FromResult(new AuthenticationState(user));
        NotifyAuthenticationStateChanged(authState);
    }

    public void NotifyUserLogout()
    {
        var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
        NotifyAuthenticationStateChanged(authState);
    }
}