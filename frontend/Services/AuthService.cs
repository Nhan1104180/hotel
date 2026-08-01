using Blazored.LocalStorage;
using frontend.ViewModel;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly CustomAuthStateProvider _authProvider;
    public string ErrorMessage { get; private set; } = "";
    public AuthService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorage, CustomAuthStateProvider authProvider)
    {
        _httpClient = httpClientFactory.CreateClient("BackEndIdentityDBService");
        _localStorage = localStorage;
        _authProvider = authProvider;
    }
    public RefreshTokenRequestViewModel _refreshTokenRequest { get; set; } = new RefreshTokenRequestViewModel();
    public LogoutRequestViewModel _logoutRequest { get; set; } = new LogoutRequestViewModel();

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();

    //Viết hàm call api https://localhost:7196/api/Auth/register
    public async Task<bool> Register(RegisterViewModel register)
    {
        var response = await _httpClient.PostAsJsonAsync("Auth/register", register);
        var result = await response.Content.ReadFromJsonAsync<ResponseData<RegisterViewModel>>();
        if (response.IsSuccessStatusCode)
        {
            ErrorMessage = "";
            register = result.Data ?? new RegisterViewModel();
            NotifyStateChanged();
            return true;
        }
        else
        {
            ErrorMessage = result?.Message ?? "Đăng ký thất bại";
            return false;
        }
    }
    //Viết hàm call api https://localhost:7196/api/Auth/login
    public async Task<bool> Login(LoginViewModel login)
    {
        var response = await _httpClient.PostAsJsonAsync("Auth/login", login);
        var result = await response.Content.ReadFromJsonAsync<ResponseData<LoginResponse>>();
        if (result != null && result.IsSuccess && result.Data != null)
        {
            var loginResult = result.Data; // Data đã là LoginResponse, không cần Deserialize lại
            ErrorMessage = "";

            // Gắn token vào header để các request sau tự động có Authorization
            await _localStorage.SetItemAsync("AccessToken", loginResult.AccessToken);
            await _localStorage.SetItemAsync("RefreshToken", loginResult.RefreshToken);

            _authProvider.NotifyUserAuthentication(loginResult.AccessToken);

            // Gắn token vào header để các request sau tự động có Authorization
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.Data.AccessToken);
            NotifyStateChanged();
            return true;
        }
        else
        {
            ErrorMessage = result?.Message ?? "Đăng nhập thất bại";

            return false;
        }
    }

    public async Task<bool> RefreshToken()
    {
        var refreshToken = await _localStorage.GetItemAsync<string>("RefreshToken");

        if (string.IsNullOrWhiteSpace(refreshToken))
            return false;

        var request = new RefreshTokenRequestViewModel
        {
            RefreshToken = refreshToken
        };

        var response = await _httpClient.PostAsJsonAsync("Auth/refresh-token", request);

        if (!response.IsSuccessStatusCode)
        {
            await Logout();
            return false;
        }

        var result = await response.Content.ReadFromJsonAsync<ResponseData<LoginResponse>>();

        if (result == null || result.Data == null)
        {
            await Logout();
            return false;
        }

        // Lưu AccessToken mới
        await _localStorage.SetItemAsync("AccessToken", result.Data.AccessToken);

        // Lưu RefreshToken mới
        await _localStorage.SetItemAsync("RefreshToken", result.Data.RefreshToken);

        NotifyStateChanged();

        return true;
    }

    public async Task<bool> Logout()
    {
        var refreshToken = await _localStorage.GetItemAsync<string>("RefreshToken");
        if (refreshToken != null)
        {
            var request = new LogoutRequestViewModel
            {
                RefreshToken = refreshToken
            };

            var response = await _httpClient.PostAsJsonAsync("Auth/logout", request);
            if (response.IsSuccessStatusCode)
            {
                // Xóa token khỏi LocalStorage dù API thành công hay không
                await _localStorage.RemoveItemAsync("AccessToken");
                await _localStorage.RemoveItemAsync("RefreshToken");

                _authProvider.NotifyUserLogout();

                NotifyStateChanged();
                return true;
            }
        }
        return false;
    }
}

