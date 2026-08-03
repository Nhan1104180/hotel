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
    public UpdateServiceViewModel _updateService { get; set; } = new UpdateServiceViewModel();
    public List<ServiceStatusViewModel> _serviceStatusList { get; set; } = new List<ServiceStatusViewModel>();
    public UpdateServiceStatusViewModel _updateServiceStatus { get; set; } = new UpdateServiceStatusViewModel();
    
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
        var response = await _httpClient.PostAsJsonAsync("Service/AddService", service);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<ResponseData<AddServiceViewModel>>();

            _addService = result.Data ?? new AddServiceViewModel();
            NotifyStateChanged();
            return true;
        }
        else
        {
            var error = await response.Content.ReadFromJsonAsync<ResponseData<object>>();
            ErrorMessage = error?.Message ?? "Có lỗi xảy ra";
            NotifyStateChanged();
            return false;
        }
    }

    //Viết hàm call api http://localhost:7219/api/Service/UpdateService/{id}
    public async Task<bool> UpdateService(int id, UpdateServiceViewModel service)
    {
        var response = await _httpClient.PutAsJsonAsync($"Service/UpdateService/{id}", service);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<ResponseData<UpdateServiceViewModel>>();

            _updateService = result.Data ?? new UpdateServiceViewModel();
            NotifyStateChanged();
            return true;
        }
        else
        {
            var error = await response.Content.ReadFromJsonAsync<ResponseData<object>>();
            ErrorMessage = error?.Message ?? "Có lỗi xảy ra";
            NotifyStateChanged();
            return false;
        }
    }

    //Viết hàm call api http://localhost:7219/api/Service/GetServiceStatus
    public async Task GetServiceStatus()
    {
        try
        {
            var response = await _httpClient.GetAsync($"Service/GetServiceStatus");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ResponseData<List<ServiceStatusViewModel>>>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                _serviceStatusList = result.Data ?? new List<ServiceStatusViewModel>();
                NotifyStateChanged();
            }
            else
            {
                _serviceStatusList = new List<ServiceStatusViewModel>();
            }
        }
        catch (HttpRequestException)
        {
            // Backend không chạy
            _serviceStatusList = new List<ServiceStatusViewModel>();
        }
        catch (Exception)
        {
            _serviceStatusList = new List<ServiceStatusViewModel>();
        }
    }

    //Viết hàm call api http://localhost:7219/api/Service/{id}/status
    public async Task<bool> UpdateServiceStatus(int id, UpdateServiceStatusViewModel service)
    {
        var response = await _httpClient.PatchAsJsonAsync($"Service/{id}/status", service);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<ResponseData<UpdateServiceStatusViewModel>>();
            _updateServiceStatus = result.Data ?? new UpdateServiceStatusViewModel();
            NotifyStateChanged();
            return true;
        }
        else
        {
            var error = await response.Content.ReadFromJsonAsync<ResponseData<object>>();
            ErrorMessage = error?.Message ?? "Có lỗi xảy ra";
            NotifyStateChanged();
            return false;
        }
    }

}
