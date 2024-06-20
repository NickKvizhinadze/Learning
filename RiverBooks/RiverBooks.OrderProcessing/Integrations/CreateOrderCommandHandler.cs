using Ardalis.Result;
using MediatR;
using RiverBooks.OrderProcessing.Contracts;
using RiverBooks.OrderProcessing.Domain;
using RiverBooks.OrderProcessing.Interfaces;

namespace RiverBooks.OrderProcessing.Integrations;

internal class CreateOrderCommandHandler(IOrderRepository orderRepository, IOrderAddressCache orderAddressCache) :
    IRequestHandler<CreateOrderCommand, Result<OrderDetailsResponse>>
{
    public async Task<Result<OrderDetailsResponse>> Handle(CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var items = request.OrderItems
            .Select(i => new OrderItem
            (
                i.BookId,
                i.Quantity,
                i.UnitPrice,
                i.Description
            )).ToList();

        // var shippingAddress = new Address("123 Main", "", "Kent", "Oh", "4444", "USA");
        // var billingAddress = shippingAddress;

        var shippingAddress = await orderAddressCache.GetByIdAsync(request.ShippingAddressId);
        var billingAddress = await orderAddressCache.GetByIdAsync(request.BillingAddressId);

        var order = Order.Factory.Create(request.UserId,
            shippingAddress.Value.Address,
            billingAddress.Value.Address,
            items);

        await orderRepository.AddAsync(order);
        await orderRepository.SaveChangesAsync();

        return Result.Success(new OrderDetailsResponse(order.Id));
    }
}