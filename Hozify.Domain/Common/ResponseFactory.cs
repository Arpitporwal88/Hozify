using Hozify.Domain.Enums;

namespace Hozify.Application.Common;

public static class ResponseFactory
{
    public static ApiResponse<T> Success<T>(
        T data,
        string message,
        ApiStatusCode statusCode = ApiStatusCode.OK)
    {
        return new ApiResponse<T>
        {
            StatusCode = (int)statusCode,
            Success = true,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> Failure<T>(
        string message,
        ApiStatusCode statusCode)
    {
        return new ApiResponse<T>
        {
            StatusCode = (int)statusCode,
            Success = false,
            Message = message,
            Data = default
        };
    }
}