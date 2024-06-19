using Ardalis.Result;
using MediatR;

namespace RiverBooks.Users.UseCases.Checkout;

public record CheckoutCartCommand(
    string? EmailAddress,
    Guid ShippingAddressId,
    Guid BillingAddressId) 
    : IRequest<Result<Guid>>;