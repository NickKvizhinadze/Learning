using MediatR;
using MongoDB.Driver;

namespace RiverBooks.EmailSending.UseCases.ListEmails;

internal class ListEmailsHandler(IMongoCollection<EmailOutboxEntity> emailCollection)
    : IRequestHandler<ListEmailsQuery, List<EmailOutboxEntity>>
{
    public async Task<List<EmailOutboxEntity>> Handle(ListEmailsQuery request, CancellationToken cancellationToken)
    {
        //TODO: Implement paging
        var filter = Builders<EmailOutboxEntity>.Filter.Empty;
        return await emailCollection.Find(filter).ToListAsync(cancellationToken: cancellationToken);
    }
}