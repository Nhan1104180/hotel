using System.Net.Http;
using System.Net.Http.Json;
using frontend.ViewModel;

public class UserService
{
    private readonly HttpClient _httpClient;
    public UserService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("BackEndUserDBService");
    }
    public UserViewModel user = new UserViewModel();
    public UserViewModel User => user; // expose ra ngoài
    public event Action? OnChange;
    private void NotifyStateChanged() => OnChange?.Invoke();

    //viết hàm call api https://localhost:7290/api/User/GetUserById/1
    public async Task GetUserById(int id)
    {
        var response = await _httpClient.GetAsync($"User/GetUserById/{id}");

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<ResponseData<UserViewModel>>();
            user = result.Data ?? new UserViewModel();
            NotifyStateChanged();
        }
        else
        {
            user = new UserViewModel();
        }
    }

    //viết hàm call api https://localhost:7290/api/User/UpdateUser/1
    public async Task UpdateUser(int id, UserViewModel userViewModel)
    {
        var response = await _httpClient.PutAsJsonAsync($"User/UpdateUser/{id}", userViewModel);
        var json = await response.Content.ReadAsStringAsync();
        Console.WriteLine(json);
        Console.WriteLine(user.Phone);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<ResponseData<UserViewModel>>();
            user = result.Data ?? new UserViewModel();
            NotifyStateChanged();
        }
        else
        {
            user = new UserViewModel();
        }
    }

}