using Ardalis.Result;
using MediatR;
using RiverBooks.OrderProcessing.Models;
using RiverBooks.OrderProcessing.Repositories;

namespace RiverBooks.OrderProcessing.UseCases;

internal class ListOrdersForUserQueryHandler(IOrderRepository userRepository)
    : IRequestHandler<ListOrdersForUserQuery, Result<List<OrderSummary>>>
{
    public async Task<Result<List<OrderSummary>>> Handle(ListOrdersForUserQuery request, CancellationToken cancellationToken)
    {
        if (request.EmailAddress is null)
            return Result.Unauthorized();

        //TODO: need to get user
        // var user = await userRepository.GetUserWithCartByEmailAsync(request.EmailAddress);
        // if (user is null)
        //     return Result.Unauthorized();
        
        //TODO: need filter by user
        var orders = await userRepository.ListAsync();

        var summaries = orders.Select(o => new OrderSummary
        {
            DateCreated = default, //o.DateCreated,
            OrderId = o.Id,
            UserId = o.UserId,
            Total = o.OrderItems.Sum(oi => oi.UnitPrice)
        }).ToList();
        return Result.Success(summaries);
    }
}