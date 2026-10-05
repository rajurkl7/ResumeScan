namespace ResumeScanApi.Application.Contracts;

public sealed record ApiResponse<T>(bool Success, string Message, T? Data, IReadOnlyCollection<string>? Errors = null)
{
    public static ApiResponse<T> Ok(T data, string message = "Request completed successfully.") => new(true, message, data);
    public static ApiResponse<T> Fail(string message, IReadOnlyCollection<string>? errors = null) => new(false, message, default, errors);
}
