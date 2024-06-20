namespace RiverBooks.OrderProcessing.Domain;

internal class Order
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid UserId { get; private set; }
    public Address ShippingAddress { get; set; } = default;
    public Address BilligAddress { get; set; } = default;

    private readonly List<OrderItem> _orderItems = new();
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    private void AddOrderItem(OrderItem item) => _orderItems.Add(item);

    
    internal class Factory
    {
        public static Order Create(Guid userId,
            Address shipping,
            Address billing,
            IEnumerable<OrderItem> orderItems)
        {
            var order = new Order
            {
                UserId = userId,
                ShippingAddress = shipping,
                BilligAddress = billing
            };

            foreach (var item in orderItems)
            {
                order.AddOrderItem(item);
            }

            return order;
        }
    }
}