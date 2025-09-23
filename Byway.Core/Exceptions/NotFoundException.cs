namespace Byway.Core.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException(string? message = null)
    {
        StatusCode = 404;
        Message = message ?? "Resource Not Found";
    }
}
