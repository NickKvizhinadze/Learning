using Ardalis.Result;
using MediatR;
using RiverBooks.Users.Entities;
using RiverBooks.Users.Models;
using RiverBooks.Users.Repositories;

namespace RiverBooks.Users.UseCases;

internal class ListCartItemQueryHandler(IApplicationUserRepository userRepository)
    : IRequestHandler<ListCartItemsQuery, Result<List<CartItemDto>>>
{
    public async Task<Result<List<CartItemDto>>> Handle(ListCartItemsQuery request, CancellationToken cancellationToken)
    {
        if (request.EmailAddress is null)
            return Result.Unauthorized();

        var user = await userRepository.GetUserWithCartByEmailAsync(request.EmailAddress);
        if (user is null)
            return Result.Unauthorized();
        
        await userRepository.SaveChangesAsync();

        return Result
            .Success(user.CartItems
                .Select(c => new CartItemDto(c.BookId, c.Description, c.Description, c.Quantity, c.UnitPrice))
                .ToList()
            );
    }
}