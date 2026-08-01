using System.Text.Json;
using frontend.ViewModel;

public class RoomTypeService
{
    private readonly HttpClient _httpClient;
    public RoomTypeService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("BackEndRoomDBService");
    }
    public List<RoomTypeViewModel> _roomType { get; set; } = new List<RoomTypeViewModel>();
    public AddRoomTypeViewModel _addRoomType { get; set; } = new AddRoomTypeViewModel();
    public UpdateRoomTypeViewModel _updateRoomType { get; set; } = new UpdateRoomTypeViewModel();
    public string ErrorMessage { get; private set; } = "";
    public event Action? OnChange;
    private void NotifyStateChanged() => OnChange?.Invoke();

    //Viết hàm call api https://localhost:7138/api/Room/GetAllRoomType
    public async Task GetRoomType()
    {
        try
        {
            var response = await _httpClient.GetAsync("RoomType/GetAllRoomType");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ResponseData<List<RoomTypeViewModel>>>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                _roomType = result.Data ?? new List<RoomTypeViewModel>();
                NotifyStateChanged();
            }
            else
            {
                _roomType = new List<RoomTypeViewModel>();
            }
        }
        catch (HttpRequestException)
        {
            // Backend không chạy
            _roomType = new List<RoomTypeViewModel>();
        }
        catch (Exception)
        {
            _roomType = new List<RoomTypeViewModel>();
        }
    }

    //Viết hàm call https://localhost:7138/api/Room/AddRoomType
    public async Task<bool> AddRoomType(AddRoomTypeViewModel roomType)
    {
        var response = await _httpClient.PostAsJsonAsync("RoomType/AddRoomType", roomType);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<ResponseData<AddRoomTypeViewModel>>();
            _addRoomType = result.Data ?? new AddRoomTypeViewModel();
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

    //Viết hàm call https://localhost:7138/api/Room/UpdateRoomType/{id}
    public async Task<bool> UpdateRoomType(int id, UpdateRoomTypeViewModel updateRoomType)
    {
        var response = await _httpClient.PutAsJsonAsync($"RoomType/UpdateRoomType/{id}", updateRoomType);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<ResponseData<UpdateRoomTypeViewModel>>();

            _updateRoomType = result.Data ?? new UpdateRoomTypeViewModel();
            NotifyStateChanged();
            return true;
        }
        else
        {
            var error = await response.Content.ReadFromJsonAsync<ResponseData<object>>();
            ErrorMessage = "Cập nhật loại phòng thất bại";
            _updateRoomType = new UpdateRoomTypeViewModel();
            return false;
        }
    }

    //Viết hàm call https://localhost:7138/api/RoomType/DeleteRoomType/{id}
    public async Task<bool> DeleteRoomType(int id)
    {
        var response = await _httpClient.DeleteAsync($"RoomType/DeleteRoomType/{id}");

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ResponseData<object>>();
            ErrorMessage = error?.Message ?? "Có lỗi xảy ra.";
            return false;
        }
        
        // Load lại danh sách
        await GetRoomType();

        NotifyStateChanged();

        return true;
    }
}