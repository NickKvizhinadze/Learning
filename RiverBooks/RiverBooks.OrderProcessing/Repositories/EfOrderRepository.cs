using Microsoft.EntityFrameworkCore;
using RiverBooks.OrderProcessing.Data;
using RiverBooks.OrderProcessing.Entities;

namespace RiverBooks.OrderProcessing.Repositories;

internal class EfOrderRepository(OrderProcessingDbContext context) : IOrderRepository
{
    public Task<List<Order>> ListAsync() 
        => context.Orders
            .Include(o => o.OrderItems)
            .ToListAsync();

    public async Task AddAsync(Order order) 
        => await context.Orders.AddAsync(order);

    public Task SaveChangesAsync()
    {
        return context.SaveChangesAsync();
    }
}