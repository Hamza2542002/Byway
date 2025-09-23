using Byway.Core.Exceptions;
namespace Byway.Presentation.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch(NotFoundException ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
        catch(BadRequestException ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
        catch(ValidationException ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
        catch (Exception ex)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = 500;
            var response = new
            {
                StatusCode = 500,
                Message = "Internal Server Error",
                Errors = new[] { ex.Message }
            };
            await httpContext.Response.WriteAsJsonAsync(response);
        }
    }

    private static Task HandleExceptionAsync(HttpContext httpContext, BaseException ex)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = ex.StatusCode;
        var response = new
        {
            ex.StatusCode,
            ex.Message,
            ex.Errors
        };
        return httpContext.Response.WriteAsJsonAsync(response);
    }
}
