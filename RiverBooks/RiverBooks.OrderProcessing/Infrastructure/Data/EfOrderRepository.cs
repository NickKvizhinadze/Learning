using Microsoft.EntityFrameworkCore;
using RiverBooks.OrderProcessing.Domain;
using RiverBooks.OrderProcessing.Interfaces;

namespace RiverBooks.OrderProcessing.Infrastructure.Data;

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