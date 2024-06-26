using FastEndpoints;
using FluentValidation;

namespace RiverBooks.Users.CartEndpoints;

public record AddCartItemRequest(Guid BookId, int Quantity);
