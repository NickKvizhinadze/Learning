using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using RiverBooks.EmailSending.Interfaces;

namespace RiverBooks.EmailSending;

internal class DefaultSendEmailsFromOutboxService(
    IOutboxService outboxService,
    ISendEmail emailSender,
    IMongoCollection<EmailOutboxEntity> emailCollection,
    ILogger<DefaultSendEmailsFromOutboxService> logger) : ISendEmailsFromOutboxService
{
    public async Task CheckForAndSendEmails()
    {
        try
        {
            var unprocessedEmailResult = await outboxService.GetUnprocessedEmailEntity();
            if (!unprocessedEmailResult.IsSuccess)
                return;

            var unprocessedEmail = unprocessedEmailResult.Value;
            await emailSender.SendEmailAsync(
                unprocessedEmail.To,
                unprocessedEmail.From,
                unprocessedEmail.Subject,
                unprocessedEmail.Body);

            var updateFilter = Builders<EmailOutboxEntity>.Filter.Eq(x => x.Id, unprocessedEmail.Id);
            var update = Builders<EmailOutboxEntity>.Update.Set("DateTimeUtcProcessed", DateTime.UtcNow);
            var updateResult = await emailCollection.UpdateOneAsync(updateFilter, update);

            logger.LogInformation("Processed {result} email records", updateResult.ModifiedCount);
        }
        finally
        {
            logger.LogInformation("Sleeping ...");
        }
    }
}