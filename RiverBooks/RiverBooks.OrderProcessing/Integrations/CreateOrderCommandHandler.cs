using Ardalis.Result;
using MediatR;
using RiverBooks.OrderProcessing.Contracts;
using RiverBooks.OrderProcessing.Entities;
using RiverBooks.OrderProcessing.Repositories;

namespace RiverBooks.OrderProcessing.Integrations;

internal class CreateOrderCommandHandler(IOrderRepository orderRepository) :
    IRequestHandler<CreateOrderCommand, Result<OrderDetailsResponse>>
{
    public async Task<Result<OrderDetailsResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var items = request.OrderItems
            .Select(i => new OrderItem
            (
                i.BookId,
                i.Quantity,
                i.UnitPrice,
                i.Description
            )).ToList();

        var shippingAddress = new Address("123 Main", "", "Kent", "Oh", "4444", "USA");
        var billingAddress = shippingAddress;
        
        var order = Order.Factory.Create(request.UserId, shippingAddress, billingAddress, items);
        
        await orderRepository.AddAsync(order);
        await orderRepository.SaveChangesAsync();

        return Result.Success(new OrderDetailsResponse(order.Id));
    }
}