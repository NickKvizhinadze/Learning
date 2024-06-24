using System.Security.Claims;
using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RiverBooks.Users.UseCases.User;
using RiverBooks.Users.UseCases.User.AddAddresses;

namespace RiverBooks.Users.UsersEndpoints.AddAddresses;

public class AddAddress(IMediator mediator) : Endpoint<AddAddressRequest>
{
    public override void Configure()
    {
        Post("/users/addresses");
        AllowAnonymous();
    }

    public override async Task HandleAsync(AddAddressRequest request, CancellationToken cancellationToken)
    {
        var emailAddress = User.FindFirstValue("EmailAddress");
        var command = new AddAddressToUserCommand(
            emailAddress,
            request.Street1,
            request.Street2,
            request.City,
            request.State,
            request.PostalCode,
            request.Country
        );
        
        var result = await mediator.Send(command, cancellationToken);
        if (result.IsUnauthorized())
            await SendUnauthorizedAsync(cancellation: cancellationToken);
        else
            await SendOkAsync(cancellation: cancellationToken);
    }
}