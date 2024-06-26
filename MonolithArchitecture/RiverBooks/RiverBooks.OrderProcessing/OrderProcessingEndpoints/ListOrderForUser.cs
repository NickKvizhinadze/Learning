using System.Security.Claims;
using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RiverBooks.OrderProcessing.UseCases;

namespace RiverBooks.OrderProcessing.OrderProcessingEndpoints;

internal class ListOrderForUser(IMediator mediator)
    : EndpointWithoutRequest<ListOrderForUserResponse>
{
    public override void Configure()
    {
        Get("/orders");
        Claims("EmailAddress");
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var email = User.FindFirstValue("EmailAddress");

        var command = new ListOrdersForUserQuery(email);

        var result = await mediator.Send(command, cancellationToken);

        if (result.Status == ResultStatus.Unauthorized)
        {
            await SendUnauthorizedAsync(cancellation: cancellationToken);
            return;
        }

        var response = new ListOrderForUserResponse
        {
            Orders = result.Value
        };
        await SendOkAsync(response, cancellation: cancellationToken);
    }
}