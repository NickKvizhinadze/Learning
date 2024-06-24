using FastEndpoints;
using FluentValidation;

namespace RiverBooks.Users.UsersEndpoints.Login;

public record UserLoginRequest(string Email, string Password);

public class UserLoginRequestValidator : Validator<UserLoginRequest>
{
    public UserLoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotNull()
            .EmailAddress()
            .WithMessage("Id must be a valid Email.");
        
        RuleFor(x => x.Password)
            .NotNull()
            .WithMessage("Password is required.");
    }
}