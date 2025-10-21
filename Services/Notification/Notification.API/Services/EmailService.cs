using SendGrid;
using SendGrid.Helpers.Mail;

namespace Notification.API.Services;

public class EmailService(IConfiguration configuration)
{
    public async Task SendEmailAsync(string sendTo, string subject, string body)
    {
        var client = new SendGridClient(configuration["SendGrid:ApiKey"]);
        var from = new EmailAddress("application.service001@gmail.com", "Project Manager");
        var to = new EmailAddress(sendTo);
        var email = MailHelper.CreateSingleEmail(from, to, subject, body, body);

        var response = await client.SendEmailAsync(email);
    }
}
