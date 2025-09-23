namespace Byway.Core.Models;

public class ServiceResultModel
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public object? Data { get; set; }
    public List<string>? Errors { get; set; }
}
