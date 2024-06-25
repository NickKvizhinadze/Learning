using Ardalis.Result.AspNetCore;
using FastEndpoints;
using RiverBooks.Books.Models;
using RiverBooks.Books.Services;

namespace RiverBooks.Books.BookEndpoints;

internal class Delete(IBookService bookService)
    : Endpoint<DeleteBookRequest>
{
    public override void Configure()
    {
        Delete("/books/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteBookRequest request, CancellationToken cancellationToken)
    {
        var result = await bookService.DeleteBookAsync(request.Id);
        if (result.IsSuccess)
            await SendNoContentAsync(cancellation: cancellationToken);
        else
            await SendAsync(result.ToMinimalApiResult(), cancellation: cancellationToken);
    }
}