﻿namespace Byway.Core.Auth;

public class AuthModel
{
    public string? Message { get; set; }

    public bool IsAuthenticated { get; set; }

    public UserDto? User { get; set; }

    public List<string>? Roles { get; set; }

    public string? Token { get; set; }

    public DateTime ExpiresOn { get; set; }
}
