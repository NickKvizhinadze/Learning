using Ardalis.Result;
using MongoDB.Driver;
using RiverBooks.EmailSending.Interfaces;

namespace RiverBooks.EmailSending;

internal class MongoDbOutboxService(IMongoCollection<EmailOutboxEntity> emailCollection) : IOutboxService
{
    public async Task QueueEmailForSending(EmailOutboxEntity entity)
    {
        await emailCollection.InsertOneAsync(entity);
    }

    public async Task<Result<EmailOutboxEntity>> GetUnprocessedEmailEntity()
    {
        var filter = Builders<EmailOutboxEntity>.Filter.Eq(x => x.DateTimeUtcProcessed, null);
        var entity = await emailCollection.Find(filter).FirstOrDefaultAsync();
        return entity is null
            ? Result<EmailOutboxEntity>.NotFound()
            : Result.Success(entity);
    }
}