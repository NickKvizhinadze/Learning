using FastEndpoints;
using RiverBooks.Books.Models;
using RiverBooks.Books.Services;

namespace RiverBooks.Books.BookEndpoints;

internal class GetById(IBookService bookService) :
    Endpoint<GetBookByIdRequest, BookDto>
{
    public override void Configure()
    {
        Get("/books/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetBookByIdRequest req, CancellationToken cancellationToken)
    {
        var book = await bookService.GetBookByIdAsync(req.Id);

        if (book is null)
        {
            await SendNotFoundAsync(cancellationToken);
            return;
        }

        await SendAsync(book, cancellation: cancellationToken);
    }
}