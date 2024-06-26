using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RiverBooks.EmailSending.EmailBackgroundService;

internal class EmailSendingBackgroundService(
    ILogger<EmailSendingBackgroundService> logger,
    ISendEmailsFromOutboxService sendEmailsFromOutboxService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var delayMilliseconds = 10_000; // 10 seconds
        logger.LogInformation("{serviceName} starting...", nameof(EmailSendingBackgroundService));

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await sendEmailsFromOutboxService.CheckForAndSendEmails();
            }
            catch (Exception ex)
            {
                logger.LogError("Error processing outbox: {message}", ex.Message);
            }
            finally
            {
                await Task.Delay(delayMilliseconds, stoppingToken);
            }
        }

        logger.LogInformation("{serviceName} stopping.", nameof(EmailSendingBackgroundService));
    }
}