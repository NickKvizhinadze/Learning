using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using RiverBooks.Books.Models;
using RiverBooks.Books.Services;

namespace RiverBooks.Books.BookEndpoints;

internal class UpdatePrice(IBookService bookService)
    : Endpoint<UpdateBookPriceRequest, BookDto>
{
    public override void Configure()
    {
        Post("/books/{Id}/PriceHistory");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateBookPriceRequest request, CancellationToken cancellationToken)
    {
        await bookService.UpdateBookPriceAsync(request.Id, request.NewPrice);
        var updatedBookResult = await bookService.GetBookByIdAsync(request.Id);
        if (updatedBookResult.IsNotFound())
            await SendNotFoundAsync(cancellation: cancellationToken);
        
        await SendAsync(updatedBookResult.Value, cancellation: cancellationToken);
    }
}