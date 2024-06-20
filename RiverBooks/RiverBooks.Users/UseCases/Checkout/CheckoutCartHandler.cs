using Ardalis.Result;
using MediatR;
using RiverBooks.OrderProcessing.Contracts;
using RiverBooks.Users.Interfaces;

namespace RiverBooks.Users.UseCases.Checkout;

internal class CheckoutCartHandler(IApplicationUserRepository userRepository, IMediator mediator)
    : IRequestHandler<CheckoutCartCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CheckoutCartCommand request, CancellationToken cancellationToken)
    {
        if (request.EmailAddress is null)
            return Result.Unauthorized();

        var user = await userRepository.GetUserWithCartByEmailAsync(request.EmailAddress);
        if (user is null)
            return Result.Unauthorized();

        var cartItems = user.CartItems
            .Select(c => new OrderItemDetails(c.BookId, c.Quantity, c.UnitPrice, c.Description))
            .ToList();

        var createOrderCommand = new CreateOrderCommand(
            Guid.Parse(user.Id), 
            request.ShippingAddressId,
            request.BillingAddressId,
            cartItems);

        //TODO: consider replacing with a message-based approach
        var orderResult = await mediator.Send(createOrderCommand, cancellationToken);
        if (!orderResult.IsSuccess) 
            return orderResult.Map(r => r.OrderId);
        
        user.ClearCart();
        await userRepository.SaveChangesAsync();

        return Result.Success(orderResult.Value.OrderId);
    }
}