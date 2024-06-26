using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RiverBooks.Users.Domain;
using RiverBooks.EmailSending.Contracts;

namespace RiverBooks.Users.UseCases.User.Create;

public class CreateUserCommandHandler(
    UserManager<ApplicationUser> userManager,
    IMediator mediator)
    : IRequestHandler<CreateUserCommand, Result>
{
    public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var newUser = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email
        };

        var result = await userManager.CreateAsync(newUser, request.Password);

        if (!result.Succeeded)
            return Result
                .Error(new ErrorList(result.Errors.Select(e => e.Description).ToArray()));

        var emailSendCommand = new SendEmailCommand
        {
            To = request.Email,
            From = "donotreply@test.ge",
            Subject = "Welcome to RiverBooks",
            Body = "Thank you for registering!"
        };

        _ = await mediator.Send(emailSendCommand, cancellationToken);

        return Result.Success();
    }
}