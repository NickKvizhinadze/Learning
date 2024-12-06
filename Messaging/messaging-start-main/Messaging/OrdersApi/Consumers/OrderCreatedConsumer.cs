using System.Text.Json;
using Contracts.Events;
using MassTransit;

namespace OrdersApi.Consumers;

public class OrderCreatedConsumer : IConsumer<OrderCreated>
{
    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        await Task.Delay(1000);
        Console.WriteLine(context.ReceiveContext.InputAddress);
        Console.WriteLine($"Consuming message: {JsonSerializer.Serialize(context.Message)}");
    }
}