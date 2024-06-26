using System.Security.Claims;
using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RiverBooks.Users.UseCases.ListItems;

namespace RiverBooks.Users.CartEndpoints;

internal class ListCartItems(IMediator mediator)
    : EndpointWithoutRequest<CartResponse>
{
    public override void Configure()
    {
        Get("/cart");
        Claims("EmailAddress");
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var email = User.FindFirstValue("EmailAddress");

        var command = new ListCartItemsQuery(email);

        var result = await mediator.Send(command, cancellationToken);

        if (result.Status == ResultStatus.Unauthorized)
        {
            await SendUnauthorizedAsync(cancellation: cancellationToken);
            return;
        }

        await SendOkAsync(new CartResponse(result.Value), cancellation: cancellationToken);
    }
}