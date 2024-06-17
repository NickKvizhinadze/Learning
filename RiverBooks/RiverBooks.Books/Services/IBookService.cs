using RiverBooks.Books.Models;

namespace RiverBooks.Books.Services;

internal interface IBookService
{
    Task<List<BookDto>> ListBooksAsync();
    Task<BookDto?> GetBookByIdAsync(Guid id);
    Task CreateBookAsync(BookDto newBook);
    Task DeleteBookAsync(Guid id);
    Task UpdateBookPriceAsync(Guid id, decimal newPrice);
}