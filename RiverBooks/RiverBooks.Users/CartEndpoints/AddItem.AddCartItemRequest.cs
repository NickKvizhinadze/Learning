using FastEndpoints;
using FluentValidation;

namespace RiverBooks.Users.CartEndpoints;

public record AddCartItemRequest(Guid BookId, int Quantity);

public class AddCartItemRequestValidator : Validator<AddCartItemRequest>
{
    public AddCartItemRequestValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity should be greater than 0.");
    }
}