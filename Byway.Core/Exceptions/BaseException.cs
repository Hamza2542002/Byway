namespace Byway.Core.Exceptions;

public class BaseException : Exception
{
    public int StatusCode { get; set; }
    public new string? Message { get; set; }
    public List<string>? Errors { get; set; }
    public BaseException()
    {
        
    }
}
