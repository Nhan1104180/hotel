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
    public AddServiceCategoryViewModel _addServiceCategory { get; set; } = new AddServiceCategoryViewModel();
    public UpdateServiceCategoryViewModel _updateServiceCategory { get; set; } = new UpdateServiceCategoryViewModel();

    public string ErrorMessage { get; private set; } = "";
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

    //Viết hàm call api http://localhost:7219/api/Category/AddCategory
    public async Task<bool> AddCategory(AddServiceCategoryViewModel category)
    {
        var response = await _httpClient.PostAsJsonAsync("Category/AddCategory", category);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<ResponseData<AddServiceCategoryViewModel>>();
            _addServiceCategory = result.Data ?? new AddServiceCategoryViewModel();
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

    //Viết hàm call api http://localhost:7219/api/Category/UpdateCategory/{id}
    public async Task<bool> UpdateCategory(int id, UpdateServiceCategoryViewModel updateCategory)
    {
        var response = await _httpClient.PutAsJsonAsync($"Category/UpdateCategory/{id}", updateCategory);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<ResponseData<UpdateServiceCategoryViewModel>>();
            _updateServiceCategory = result.Data ?? new UpdateServiceCategoryViewModel();
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

    //Viết hàm call api http://localhost:7219/api/Category/DeleteCategory/{id}
    public async Task<bool> DeleteCategory(int id)
    {
        var response = await _httpClient.DeleteAsync($"Category/DeleteCategory/{id}");

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ResponseData<object>>();

            ErrorMessage = error?.Message ?? "Có lỗi xảy ra.";
            return false;
        }

        // Load lại danh sách
        await GetServiceCategory();

        NotifyStateChanged();
        
        return true;
    }
}