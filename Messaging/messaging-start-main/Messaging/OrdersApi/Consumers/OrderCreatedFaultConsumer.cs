using System.Text.Json;
using Contracts.Events;
using MassTransit;

namespace OrdersApi.Consumers;

public class OrderCreatedFaultConsumer : IConsumer<Fault<OrderCreated>>
{
    public async Task Consume(ConsumeContext<Fault<OrderCreated>> context)
    {
        await Task.Delay(1000);
        Console.WriteLine(context.ReceiveContext.InputAddress);
        Console.WriteLine($"Consuming faulted message: {JsonSerializer.Serialize(context.Message)}");
    }
}