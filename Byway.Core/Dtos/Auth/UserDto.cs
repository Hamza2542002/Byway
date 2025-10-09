namespace Byway.Core.Auth;

public class UserDto
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Image { get; set; }
    public string? Role { get; set; }
}
