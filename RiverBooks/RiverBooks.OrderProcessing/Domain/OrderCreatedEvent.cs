using RiverBooks.SharedKernel;

namespace RiverBooks.OrderProcessing.Domain;

internal class OrderCreatedEvent: DomainEventBase
{
    public Order Order { get; }

    public OrderCreatedEvent(Order order)
    {
        Order = order;
    }
    
}