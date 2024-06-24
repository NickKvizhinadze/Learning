using MediatR;

namespace RiverBooks.OrderProcessing.Contracts;

public class OrderCreatedIntegrationEvent(OrderDetailsDto orderDetails)
    : INotification
{
    public DateTimeOffset DateCreated { get; private set; } = DateTimeOffset.UtcNow;
    public OrderDetailsDto OrderDetails { get; private set; } = orderDetails;
}