using Contracts.Responses;
using MassTransit;
using Orders.Domain.Entities;
using Orders.Service;

namespace OrdersApi.Consumers;

public class VerifyOrderConsumer : IConsumer<VerifyOrder>
{
    private readonly IOrderService _orderService;

    public VerifyOrderConsumer(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task Consume(ConsumeContext<VerifyOrder> context)
    {
        var order = await _orderService.GetOrderAsync(context.Message.Id);
        if (order is not null)
        {
            await context.RespondAsync(new OrderResult
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Status = order.Status
            });
        }
        else
        {
            await context.RespondAsync(new OrderNotFoundResult
            {
                ErrorMessage = "Sorry your order is lost!"
            });
        }
    }
}