namespace GMS.Application.Common.Models;

public sealed class ApiResponse<T>
{
    public bool Success { get; init; }
    public int StatusCode { get; init; }
    public T? Data { get; init; }
    public string Message { get; init; } = string.Empty;
    public IReadOnlyCollection<string> Errors { get; init; } = Array.Empty<string>();

    public static ApiResponse<T> Ok(T data, string message = "Operation successful", int statusCode = 200) =>
        new()
        {
            Success = true,
            StatusCode = statusCode,
            Data = data,
            Message = message,
            Errors = Array.Empty<string>()
        };

    public static ApiResponse<T> Fail(string message, IEnumerable<string>? errors = null, int statusCode = 400) =>
        new()
        {
            Success = false,
            StatusCode = statusCode,
            Data = default,
            Message = message,
            Errors = errors?.ToArray() ?? Array.Empty<string>()
        };
}
