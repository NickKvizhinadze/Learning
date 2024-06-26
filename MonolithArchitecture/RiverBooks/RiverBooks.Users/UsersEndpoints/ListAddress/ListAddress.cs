using System.Security.Claims;
using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RiverBooks.Users.UseCases.User;
using RiverBooks.Users.UseCases.User.ListAddress;

namespace RiverBooks.Users.UsersEndpoints.ListAddress;

public class ListAddress(IMediator mediator) : EndpointWithoutRequest<ListAddressResponse>
{
    public override void Configure()
    {
        Get("/users/addresses");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var emailAddress = User.FindFirstValue("EmailAddress");

        var query = new ListUserAddressQuery(emailAddress);

        var result = await mediator.Send(query, cancellationToken);

        if (result.IsUnauthorized())
            await SendUnauthorizedAsync(cancellation: cancellationToken);
        else
            await SendOkAsync(new ListAddressResponse(result.Value), cancellationToken);
    }
}