using RiverBooks.OrderProcessing.Models;

namespace RiverBooks.OrderProcessing.OrderProcessingEndpoints;

public class ListOrderForUserResponse
{
    public List<OrderSummary> Orders { get; set; } = new();
}