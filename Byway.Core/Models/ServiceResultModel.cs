namespace Byway.Core.Models;

public class ServiceResultModel<T>
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }

    public static ServiceResultModel<T> Success(T? data, string message = "")
    {
        return new ServiceResultModel<T>
        {
            IsSuccess = true,
            Message = message,
            Data = data
        };
    }
    public static ServiceResultModel<T> Failure(string message)
    {
        return new ServiceResultModel<T>
        {
            IsSuccess = false,
            Message = message,
            Data = default
        };
    }
}
