using System;
using System.Text.Json;
using System.Threading.Tasks;
using MassTransit;
using Contracts.Events;

namespace AdminNotifications.Worker;

public class OrderCreatedConsumer: IConsumer<OrderCreated>
{
    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        await Task.Delay(1000);
        Console.WriteLine(context.ReceiveContext.InputAddress);
        Console.WriteLine($"Admin Consuming message: {JsonSerializer.Serialize(context.Message)}");
    }
}