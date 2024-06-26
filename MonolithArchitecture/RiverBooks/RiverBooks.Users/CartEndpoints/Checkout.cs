using System.Security.Claims;
using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RiverBooks.Users.UseCases.Checkout;

namespace RiverBooks.Users.CartEndpoints;

internal class Checkout(IMediator mediator)
    : Endpoint<CheckoutRequest, CheckoutResponse>
{
    public override void Configure()
    {
        Post("/cart/checkout");
        Claims("EmailAddress");
    }

    public override async Task HandleAsync(CheckoutRequest request, CancellationToken cancellationToken)
    {
        var email = User.FindFirstValue("EmailAddress");

        var command = new CheckoutCartCommand(
            email,
            request.ShippingAddressId,
            request.BillingAddressId);

        var result = await mediator.Send(command, cancellationToken);

        if (result.Status == ResultStatus.Unauthorized)
            await SendUnauthorizedAsync(cancellation: cancellationToken);
        else
            await SendOkAsync(cancellation: cancellationToken);
    }
}