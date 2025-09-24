using Byway.Core.Helpers;
using Byway.Core.IServices;
using Byway.Core.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Byway.Application.Services;

public class EmailService : IEmailService
{
    private readonly EmailConfiguration _emailSettings;
    public EmailService(IOptions<EmailConfiguration> mailOptions)
    {
        _emailSettings = mailOptions.Value;
    }
    public async Task<bool> SendEmailAsync(Email email, bool isHtmlBody = true)
    {
        var smtpClient = new SmtpClient
        {
            EnableSsl = _emailSettings.EnableSsl,
            Host = _emailSettings.Host ?? "smtp.gmail.com",
            Port = _emailSettings.Port,
            Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password)
        };

        var emailMessage = new MailMessage(
            _emailSettings.Email!,
            email.To!,
            email.Subject!,
            email.Body!
            )
        {
            IsBodyHtml = isHtmlBody
        };
        try
        {
            await smtpClient.SendMailAsync(emailMessage);
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            emailMessage.Dispose();
            smtpClient.Dispose();
        }
        return true;
    }
}
