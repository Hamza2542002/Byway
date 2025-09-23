namespace Byway.Core.Exceptions;

public class ValidationException : BaseException
{
    public ValidationException(string? message = null)
    {
        StatusCode = 400;
        Message = message ?? "Error Validating Data";
    }
}
