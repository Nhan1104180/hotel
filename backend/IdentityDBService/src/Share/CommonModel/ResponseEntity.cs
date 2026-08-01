namespace Share.CommonModel;

public class ResponseEntity
{
    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public object? Data { get; set; }

}