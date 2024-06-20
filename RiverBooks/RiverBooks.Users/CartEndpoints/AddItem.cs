using System.Security.Claims;
using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RiverBooks.Users.UseCases;
using RiverBooks.Users.UseCases.AddItem;

namespace RiverBooks.Users.CartEndpoints;

internal class AddItem(IMediator mediator) 
    : Endpoint<AddCartItemRequest>
{
    public override void Configure()
    {
        Post("/cart");
        Claims("EmailAddress");
    }

    public override async Task HandleAsync(AddCartItemRequest request, CancellationToken cancellationToken)
    {
        var email = User.FindFirstValue("EmailAddress");

        var command = new AddItemToCartCommand(request.BookId, request.Quantity, email);
        
        var result = await mediator.Send(command, cancellationToken);

        if (result.Status == ResultStatus.Unauthorized)
            await SendUnauthorizedAsync(cancellation: cancellationToken);
        else
            await SendOkAsync(cancellation: cancellationToken);
    }
}