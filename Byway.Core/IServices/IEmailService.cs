using Byway.Core.Models;

namespace Byway.Core.IServices;

public interface IEmailService
{
    Task<bool> SendEmailAsync(Email email, bool isHtmlBody = true);
}
