using FastEndpoints;
using RiverBooks.Books.Models;
using RiverBooks.Books.Services;

namespace RiverBooks.Books.BookEndpoints;

internal class Create(IBookService bookService) 
    : Endpoint<CreateBookRequest, BookDto>
{
    public override void Configure()
    {
        Post("/books");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateBookRequest request, CancellationToken cancellationToken)
    {
        var book = new BookDto(Guid.NewGuid(),request.Title, request.Author, request.Price);
        await bookService.CreateBookAsync(book);

        await SendAsync(book, cancellation: cancellationToken);
    }
}