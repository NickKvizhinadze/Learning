using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.Events;
using Contracts.Models;
using MassTransit;
using Orders.Domain.Entities;
using Orders.Service;

namespace OrderCreation.Worker;

public class OrderCreateConsumer: IConsumer<OrderModel>
{
    private readonly IMapper _mapper;
    private readonly IOrderService _orderService;

    public OrderCreateConsumer(IMapper mapper, IOrderService orderService)
    {
        _mapper = mapper;
        _orderService = orderService;
    }

    public async Task Consume(ConsumeContext<OrderModel> context)
    {
        Console.WriteLine($"Got message for creating the order: {context.Message}");
        var orderToAdd = _mapper.Map<Order>(context.Message);
        var createdOrder = await _orderService.AddOrderAsync(orderToAdd);

        await context.Publish(new OrderCreated
            {
                Id = createdOrder.Id,
                CreatedAt = createdOrder.OrderDate,
                OrderId = createdOrder.OrderId,
                TotalAmount = createdOrder.OrderItems.Sum(i => i.Price * i.Quantity)
            }, c =>
            {
                c.Headers.Set("my-custom-header", "value");
                // c.TimeToLive = TimeSpan.FromSeconds(30);
            }
        );
        
        
    }
}