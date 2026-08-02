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
    public AddServiceViewModel _addService { get; set; } = new AddServiceViewModel();
    public string ErrorMessage { get; private set; } = "";
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

    //Viết hàm call api http://localhost:7219/api/Service/AddService
    public async Task<bool> AddService(AddServiceViewModel service)
    {
        Console.WriteLine("Đang gọi API AddService...");

        Console.WriteLine("Data gửi:");

        Console.WriteLine(JsonSerializer.Serialize(service));

        var response = await _httpClient.PostAsJsonAsync("Service/AddService", service);

        Console.WriteLine($"Status Code: {(int)response.StatusCode} - {response.StatusCode}");

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<ResponseData<AddServiceViewModel>>();

            Console.WriteLine("Thêm Service thành công!");
            Console.WriteLine($"Message: {result?.Message}");

            _addService = result.Data ?? new AddServiceViewModel();
            NotifyStateChanged();
            return true;
        }
        else
        {
            var error = await response.Content.ReadFromJsonAsync<ResponseData<object>>();
            Console.WriteLine("Thêm Service thất bại!");
            Console.WriteLine($"Lỗi: {error?.Message}");

            ErrorMessage = error?.Message ?? "Có lỗi xảy ra";
            NotifyStateChanged();
            return false;
        }
    }



}
