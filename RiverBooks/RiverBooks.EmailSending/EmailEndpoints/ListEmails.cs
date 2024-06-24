using FastEndpoints;
using MediatR;
using MongoDB.Driver;
using RiverBooks.EmailSending.UseCases.ListEmails;

namespace RiverBooks.EmailSending.EmailEndpoints;

internal class ListEmails(IMediator mediator) : EndpointWithoutRequest<ListEmailsResponse>
{
    public override void Configure()
    {
        Get("/emails");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var query = new ListEmailsQuery();
        var emails = await mediator.Send(query, cancellationToken);

        await SendOkAsync(new ListEmailsResponse
        {
            Count = emails.Count,
            Emails = emails //TODO: Map to dto
        }, cancellation: cancellationToken);
    }
}