namespace RiverBooks.OrderProcessing.Contracts;

public class OrderDetailsDto
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public DateTime DateCreated { get; set; }
    public List<OrderItemDetails> OrderItems { get; set; } = new();
}