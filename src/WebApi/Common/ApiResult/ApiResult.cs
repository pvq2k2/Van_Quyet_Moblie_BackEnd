namespace WebApi.Common.ApiResult;

/// <summary>
/// Chuẩn hóa Response API trả về.
/// </summary>
/// <typeparam name="T">Kiểu dữ liệu của Data.</typeparam>
public class ApiResult<T>
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public object? ErrorDetails { get; set; }

    public ApiResult()
    {
    }

    public ApiResult(bool success, int statusCode, string message, T? data = default)
    {
        Success = success;
        StatusCode = statusCode;
        Message = message;
        Data = data;
    }

    public static ApiResult<T> SuccessResult(T data, string message = "Success", int statusCode = 200)
    {
        return new ApiResult<T>(true, statusCode, message, data);
    }

    public static ApiResult<T> FailureResult(string message, int statusCode, object? errorDetails = null)
    {
        return new ApiResult<T>
        {
            Success = false,
            StatusCode = statusCode,
            Message = message,
            ErrorDetails = errorDetails,
            Data = default
        };
    }
}
