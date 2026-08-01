namespace frontend.ViewModel;

public class ResponseData<T>
{
    public T? Data { get; set; }
    public string? Message { get; set; }
    public bool IsSuccess { get; set; }
}