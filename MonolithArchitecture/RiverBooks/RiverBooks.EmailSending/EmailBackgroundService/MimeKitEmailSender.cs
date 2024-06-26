using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace RiverBooks.EmailSending.EmailBackgroundService;

public class MimeKitEmailSender(ILogger<MimeKitEmailSender> logger) : ISendEmail
{
    public async Task SendEmailAsync(string to, string from, string subject, string body)
    {
        logger.LogInformation("Attempting to sending email to {To} from {From} with subject {Subject}", to, from,
            subject);

        using var client = new SmtpClient();
        await client.ConnectAsync(Constants.EMAIL_SERVER, 25, false);
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(from, from));
        message.To.Add(new MailboxAddress(to, to));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };
        
        await client.SendAsync(message);
        
        logger.LogInformation("Email sent!");
        await client.DisconnectAsync(true);
    }
}