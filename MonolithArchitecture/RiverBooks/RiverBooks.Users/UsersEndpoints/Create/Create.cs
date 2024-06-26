using Ardalis.Result.AspNetCore;
using FastEndpoints;
using MediatR;
using RiverBooks.Users.UseCases.User;
using RiverBooks.Users.UseCases.User.Create;

namespace RiverBooks.Users.UsersEndpoints.Create;

internal class Create(IMediator mediator)
    : Endpoint<CreateUserRequest>
{
    public override void Configure()
    {
        Post("/users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateUserCommand(request.Email, request.Password);
        var result = await mediator.Send(command, cancellationToken);
        if (!result.IsSuccess)
            await SendResultAsync(result.ToMinimalApiResult());
        else
            await SendOkAsync(cancellation: cancellationToken);
    }
}