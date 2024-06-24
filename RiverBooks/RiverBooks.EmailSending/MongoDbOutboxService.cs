using MongoDB.Driver;
using RiverBooks.EmailSending.Interfaces;

namespace RiverBooks.EmailSending;

internal class MongoDbOutboxService(IMongoCollection<EmailOutboxEntity> emailCollection): IOutboxService
{
    public async Task QueueEmailForSending(EmailOutboxEntity entity)
    {
        await emailCollection.InsertOneAsync(entity);
        
    }
}