using FastEndpoints;
using FluentValidation;

namespace RiverBooks.Books.BookEndpoints;

public record CreateBookRequest(Guid? Id, string Title, string Author, decimal Price);

public class CreateBookRequestValidator : Validator<CreateBookRequest>
{
    public CreateBookRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .NotEqual(Guid.Empty)
            .WithMessage("Id must be a valid Guid.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Book price must not be negative.");
    }
}