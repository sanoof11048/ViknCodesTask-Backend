namespace ViknCodesTask.Common
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public string? Error { get; set; }
        public bool Success => StatusCode >= 200 && StatusCode < 300;

        public ApiResponse(int statuscode, string message, T? data = default(T), string? error = null)
        {
            StatusCode = statuscode;
            Message = message;
            Data = data;
            Error = error;
        }
    }
}
