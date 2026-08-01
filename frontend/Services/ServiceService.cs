using System.Text.Json;
using frontend.ViewModel;

public class ServiceService
{
    private readonly HttpClient _httpClient;
    public ServiceService(IHttpClientFactory httpClientFactory)
    {
       _httpClient = httpClientFactory.CreateClient("BackEndServiceDBService");
    }
    public List<ServiceViewModel> _serviceList { get; set; } = new List<ServiceViewModel>();
    public event Action? OnChange;
    private void NotifyStateChanged() => OnChange?.Invoke();

    //Viết hàm call api http://localhost:7219/api/Service/GetAllServices
    public async Task GetServiceList()
    {
        try
        {
            var response = await _httpClient.GetAsync($"Service/GetAllServices");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ResponseData<List<ServiceViewModel>>>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                _serviceList = result.Data ?? new List<ServiceViewModel>();
                NotifyStateChanged();
            }
            else
            {
                _serviceList = new List<ServiceViewModel>();
            }
        }
        catch (HttpRequestException)
        {
            // Backend không chạy
            _serviceList = new List<ServiceViewModel>();
        }
        catch (Exception)
        {
            _serviceList = new List<ServiceViewModel>();
        }
    }
    
   


}
