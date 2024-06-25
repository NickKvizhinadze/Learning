using FastEndpoints;
using MediatR;
using MongoDB.Driver;
using RiverBooks.EmailSending.UseCases.ListEmails;

namespace RiverBooks.EmailSending.EmailEndpoints;

internal record EmailOutboxDto(Guid Id,
    string To,
    string From,
    string Subject,
    string Body,
    DateTime? DateTimeProcessed);

internal class ListEmails(IMediator mediator) : Endpoint<ListEmailsRequest, ListEmailsResponse>
{
    public override void Configure()
    {
        Get("/emails");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ListEmailsRequest request, CancellationToken cancellationToken)
    {
        var query = new ListEmailsQuery(request.Page, request.PageSize);
        var emailsResult = await mediator.Send(query, cancellationToken);

        await SendOkAsync(new ListEmailsResponse
        {
            Count = emailsResult.count,
            Emails = emailsResult.emails
                .Select(e => 
                    new EmailOutboxDto(e.Id, e.To, e.From, e.Subject, e.Body, e.DateTimeUtcProcessed))
                .ToList()
        }, cancellation: cancellationToken);
    }
}