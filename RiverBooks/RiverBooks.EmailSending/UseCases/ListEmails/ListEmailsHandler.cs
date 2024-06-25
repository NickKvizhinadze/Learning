using MediatR;
using MongoDB.Driver;

namespace RiverBooks.EmailSending.UseCases.ListEmails;

internal class EmailOutboxEntityListWrapper
{
    public long Count { get; set; }
    public List<EmailOutboxEntity> Emails { get; set; } = new();
}

internal class ListEmailsHandler(IMongoCollection<EmailOutboxEntity> emailCollection)
    : IRequestHandler<ListEmailsQuery, (List<EmailOutboxEntity> emails, long count)>
{
    public async Task<(List<EmailOutboxEntity> emails, long count)> Handle(ListEmailsQuery request,
        CancellationToken cancellationToken)
    {
        var filter = Builders<EmailOutboxEntity>.Filter.Empty;
        var query = emailCollection.Find(filter);
        var count = await query.CountDocumentsAsync(cancellationToken: cancellationToken);

        var emails = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Limit(request.PageSize)
            .ToListAsync(cancellationToken: cancellationToken);

        return (emails, count);
    }
}