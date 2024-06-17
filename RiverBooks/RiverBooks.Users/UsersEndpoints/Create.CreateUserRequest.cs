using FastEndpoints;
using FluentValidation;

namespace RiverBooks.Users.UsersEndpoints;

public record CreateUserRequest(string Email, string Password);

public class CreateBookRequestValidator : Validator<CreateUserRequest>
{
    public CreateBookRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotNull()
            .EmailAddress()
            .WithMessage("Id must be a valid Email.");
    }
}