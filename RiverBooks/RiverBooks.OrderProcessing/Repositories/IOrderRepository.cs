using RiverBooks.OrderProcessing.Entities;

namespace RiverBooks.OrderProcessing.Repositories;

internal interface IOrderRepository
{
    Task<List<Order>> ListAsync();
    Task AddAsync(Order order);
    Task SaveChangesAsync();
}