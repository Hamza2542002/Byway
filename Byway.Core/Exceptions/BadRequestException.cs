namespace Byway.Core.Exceptions;

public class BadRequestException : BaseException
{
    public BadRequestException(string? message = null)
    {
        StatusCode = 400;
        Message = message ?? "Bad Request";
    }
}
