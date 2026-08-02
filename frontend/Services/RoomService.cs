using System.Text.Json;
using frontend.ViewModel;

public class RoomService
{
    private readonly HttpClient _httpClient;
    public RoomService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("BackEndRoomDBService");
    }
    public List<RoomViewModel> _roomList { get; set; } = new List<RoomViewModel>();
    public RoomDetailViewModel _roomDetail { get; set; } = new RoomDetailViewModel();
    public AddRoomViewModel _addRoom { get; set; } = new AddRoomViewModel();
    public UpdateRoomViewModel _updateRoom { get; set; } = new UpdateRoomViewModel();
    public List<RoomStatusViewModel> _roomStatusList { get; set; } = new List<RoomStatusViewModel>();
    public UpdateRoomStatusViewModel _updateRoomStatus { get; set; } = new UpdateRoomStatusViewModel();
    
    public string ErrorMessage { get; private set; } = "";
    public event Action? OnChange;
    private void NotifyStateChanged() => OnChange?.Invoke();

    //Viết hàm call api http://localhost:7138/api/Room/GetAllRooms?pageIndex=1&pageSize=10
    public async Task GetRoomList()
    {
        try
        {
            var response = await _httpClient.GetAsync($"Room/GetAllRooms");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ResponseData<List<RoomViewModel>>>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                _roomList = result.Data ?? new List<RoomViewModel>();
                NotifyStateChanged();
            }
            else
            {
                _roomList = new List<RoomViewModel>();
            }
        }
        catch (HttpRequestException)
        {
            // Backend không chạy
            _roomList = new List<RoomViewModel>();
        }
        catch (Exception)
        {
            _roomList = new List<RoomViewModel>();
        }
    }


    //Viết hàm call http://localhost:7138/api/Room/GetRoomById/1
    public async Task GetRoomDetail(int id)
    {
        var response = await _httpClient.GetAsync($"Room/GetRoomById/{id}");

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<ResponseData<RoomDetailViewModel>>();
            _roomDetail = result.Data ?? new RoomDetailViewModel();
            NotifyStateChanged();
        }
        else
        {
            _roomList = new List<RoomViewModel>();
        }
    }

    //Viết hàm call http://localhost:7138/api/Room/AddRoom
    public async Task<bool> AddRoom(AddRoomViewModel room)
    {
        var response = await _httpClient.PostAsJsonAsync("Room/AddRoom", room);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<ResponseData<AddRoomViewModel>>();
            _addRoom = result.Data ?? new AddRoomViewModel();
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

    //Viết hàm call https://localhost:7138/api/Room/UpdateRoom/{id}
    public async Task<bool> UpdateRoom(int id, UpdateRoomViewModel room)
    {
        var response = await _httpClient.PutAsJsonAsync($"Room/UpdateRoom/{id}", room);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<ResponseData<UpdateRoomViewModel>>();
            _updateRoom = result.Data ?? new UpdateRoomViewModel();
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

    //Viết hàm callhttps://localhost:7138/api/Room/DeleteRoom/{id}
    public async Task<bool> DeleteRoom(int id)
    {
        var response = await _httpClient.DeleteAsync($"Room/DeleteRoom/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        // Load lại danh sách
        await GetRoomList();

        NotifyStateChanged();

        return true;
    }

    //Viết hàm call http://localhost:5257/api/Room/GetRoomStatus
    public async Task GetRoomStatus()
    {
        try
        {
            var response = await _httpClient.GetAsync($"Room/GetRoomStatus");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ResponseData<List<RoomStatusViewModel>>>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                _roomStatusList = result.Data ?? new List<RoomStatusViewModel>();
                NotifyStateChanged();
            }
            else
            {
                _roomStatusList = new List<RoomStatusViewModel>();
            }
        }
        catch (HttpRequestException)
        {
            // Backend không chạy
            _roomStatusList = new List<RoomStatusViewModel>();
        }
        catch (Exception)
        {
            _roomStatusList = new List<RoomStatusViewModel>();
        }
    }

    //Viết hàm call http://localhost:7138/api/Room/SearchRoom?keyword=VIP&pageIndex=1&pageSize=10
    public async Task SearchRoom(string keyword, int pageNumber, int pageSize)
    {
        var response = await _httpClient.GetAsync($"Room/SearchRoom?keyword={keyword}&pageIndex={pageNumber}&pageSize={pageSize}");

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<ResponseData<List<RoomViewModel>>>();

            _roomList = result.Data ?? new List<RoomViewModel>();
            NotifyStateChanged();
        }
        else
        {
            var error = await response.Content.ReadFromJsonAsync<ResponseData<object>>();
            _roomList = new List<RoomViewModel>();
            ErrorMessage = error?.Message ?? "Không tìm thấy phòng";
        }
    }

    //Viết hàm call https://localhost:7138/api/Room/1004/status
    public async Task<bool> UpdateRoomStatus(int id, UpdateRoomStatusViewModel status)
    {
        try
        {
            var response = await _httpClient.PatchAsJsonAsync($"Room/{id}/status", status);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ResponseData<object>>();
                ErrorMessage = error?.Message ?? "Có lỗi xảy ra";

                _updateRoomStatus = new UpdateRoomStatusViewModel();
                NotifyStateChanged();

                return false;
            }

            // Load lại danh sách phòng sau khi cập nhật thành công
            await GetRoomList();

            NotifyStateChanged();

            return true;
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Không thể kết nối tới máy chủ.";
            _updateRoomStatus = new UpdateRoomStatusViewModel();
            NotifyStateChanged();

            return false;
        }
        catch (Exception)
        {
            ErrorMessage = "Đã xảy ra lỗi.";
            _updateRoomStatus = new UpdateRoomStatusViewModel();
            NotifyStateChanged();

            return false;
        }
    }
}