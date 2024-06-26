using Ardalis.Result;
using FastEndpoints;
using FluentValidation;
using MediatR;

namespace RiverBooks.Users.UseCases.User.Create;

public record CreateUserCommand(string Email, string Password) : IRequest<Result>;

public class CreateUserCommandValidator : Validator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotNull()
            .EmailAddress()
            .WithMessage("Id must be a valid Email.");
        
        RuleFor(x => x.Password)
            .NotNull()
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters.");
    }
}