public class ApiResponse<T>
{
    public int StatusCode { get; set; }

    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public T? Data { get; set; }

    public Dictionary<string, string[]>? Errors { get; set; }
}