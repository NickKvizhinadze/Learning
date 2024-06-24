using Ardalis.Result;
using MediatR;
using RiverBooks.EmailSending.Contracts;
using RiverBooks.EmailSending.Interfaces;

namespace RiverBooks.EmailSending.Integrations;

internal class QueueEmailToOutboxSendEmailCommandHandler(IOutboxService outboxService) : IRequestHandler<SendEmailCommand, Result<Guid>>
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