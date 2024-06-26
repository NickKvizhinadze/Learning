using Ardalis.Result;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace RiverBooks.EmailSending.EmailBackgroundService;

internal interface IGetEmailsFromOutboxService
{
    Task<Result<EmailOutboxEntity>> GetUnprocessedEmailEntity();
}

internal class MongoDbGetEmailsFromOutboxService(IMongoCollection<EmailOutboxEntity> emailCollection) : IGetEmailsFromOutboxService
{
    public async Task<Result<EmailOutboxEntity>> GetUnprocessedEmailEntity()
    {
        var filter = Builders<EmailOutboxEntity>.Filter.Eq(x => x.DateTimeUtcProcessed, null);
        var entity = await emailCollection.Find(filter).FirstOrDefaultAsync();
        return entity is null
            ? Result<EmailOutboxEntity>.NotFound()
            : Result.Success(entity);
    }
}

internal class DefaultSendEmailsFromOutboxService(
    IGetEmailsFromOutboxService outboxService,
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