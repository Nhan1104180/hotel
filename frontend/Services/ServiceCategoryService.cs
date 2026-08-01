using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using frontend.ViewModel;

public class ServiceCategoryService
{
    private readonly HttpClient _httpClient;
    public ServiceCategoryService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("BackEndServiceDBService");
    }
    public List<ServiceCategoryViewModel> _serviceCategory { get; set; } = new List<ServiceCategoryViewModel>();

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
    //Viết hàm call api http://localhost:7219/api/Category/GetAllCategory
    public async Task GetServiceCategory()
    {
        try
        { 
            var response = await _httpClient.GetAsync("Category/GetAllCategory");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ResponseData<List<ServiceCategoryViewModel>>>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                _serviceCategory = result.Data ?? new List<ServiceCategoryViewModel>();
                NotifyStateChanged();
            }
            else
            {
                _serviceCategory = new List<ServiceCategoryViewModel>();
            }
        }
        catch (HttpRequestException)
        {
            // Backend không chạy
            _serviceCategory = new List<ServiceCategoryViewModel>();
        }
        catch (Exception)
        {
            _serviceCategory = new List<ServiceCategoryViewModel>();
        }
    }


    // Place your HTTP methods below
}