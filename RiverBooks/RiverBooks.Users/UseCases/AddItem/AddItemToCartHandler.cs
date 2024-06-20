using Ardalis.Result;
using MediatR;
using RiverBooks.Books.Contracts;
using RiverBooks.Users.Domain;
using RiverBooks.Users.Interfaces;

namespace RiverBooks.Users.UseCases.AddItem;

internal class AddItemToCartHandler(IApplicationUserRepository userRepository, IMediator mediator)
    : IRequestHandler<AddItemToCartCommand, Result>
{
    public async Task<Result> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        if (request.EmailAddress is null)
            return Result.Unauthorized();

        var user = await userRepository.GetUserWithCartByEmailAsync(request.EmailAddress);
        if (user is null)
            return Result.Unauthorized();

        var bookQuery = new BookDetailsQuery(request.BookId);
        var bookResult = await mediator.Send(bookQuery, cancellationToken);
        
        if(bookResult.Status == ResultStatus.NotFound)
            return Result.NotFound();

        var bookDetails = bookResult.Value;
        //TODO: Get description and price from Books module
        var description = $"{bookDetails.Title} by {bookDetails.Author}";
        var newCartItem = new CartItem(request.BookId, description, request.Quantity, bookDetails.Price);
        
        user.AddToCart(newCartItem);

        await userRepository.SaveChangesAsync();

        return Result.Success();
    }
}