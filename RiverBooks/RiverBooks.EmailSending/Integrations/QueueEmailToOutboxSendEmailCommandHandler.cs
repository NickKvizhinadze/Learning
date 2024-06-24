using Ardalis.Result;
using MediatR;
using MongoDB.Driver;
using RiverBooks.EmailSending.Contracts;

namespace RiverBooks.EmailSending.Integrations;

internal interface IQueueEmailsInOutboxService
{
    Task QueueEmailForSending(EmailOutboxEntity entity);
}

internal class MongoDbQueueEmailsOutboxService(IMongoCollection<EmailOutboxEntity> emailCollection)
    : IQueueEmailsInOutboxService
{
    public async Task QueueEmailForSending(EmailOutboxEntity entity)
    {
        await emailCollection.InsertOneAsync(entity);
    }
}

internal class QueueEmailToOutboxSendEmailCommandHandler(IQueueEmailsInOutboxService outboxService) : IRequestHandler<SendEmailCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        var entity = new EmailOutboxEntity
        {
            To = request.To,
            From = request.From,
            Subject = request.Subject,
            Body = request.Body
        };
        await outboxService.QueueEmailForSending(entity);
        // await emailSender.SendEmailAsync(request.To, request.From, request.Subject, request.Body);
        return Result.Success(entity.Id);
    }
}