using FastEndpoints;
using RiverBooks.Books.BookEndpoints;
using RiverBooks.Books.Services;

namespace RiverBooks.Books.BookEndpoints;

internal class List(IBookService bookService) : EndpointWithoutRequest<ListBooksResponse>
{
    public override void Configure()
    {
        Get("/books");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var books = await bookService.ListBooksAsync();

        await SendAsync(new ListBooksResponse
        {
            Books = books
        }, cancellation: cancellationToken);
    }
}