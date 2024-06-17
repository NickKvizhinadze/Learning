using RiverBooks.Books.Models;
using RiverBooks.Books.Entities;
using RiverBooks.Books.Repositories;

namespace RiverBooks.Books.Services;

internal class BookService(IBookRepository bookRepository) : IBookService
{
    public async Task<List<BookDto>> ListBooksAsync()
    {
        var books = (await bookRepository.ListAsync())
            .Select(b => new BookDto(b.Id, b.Title, b.Author, b.Price))
            .ToList();

        return books;

        // return
        // [
        //     new BookDto(Guid.NewGuid(), "The Fellowship of the Ring", "J.R.R. Tolkein"),
        //     new BookDto(Guid.NewGuid(), "The Two Towers", "J.R.R. Tolkein"),
        //     new BookDto(Guid.NewGuid(), "The Return of the King", "J.R.R. Tolkein")
        // ];
    }

    public async Task<BookDto?> GetBookByIdAsync(Guid id)
    {
        var book = await bookRepository.GetByIdAsync(id);
        //TODO: handle not found case
        return book is null 
            ? null 
            : new BookDto(book.Id, book.Title, book.Author, book.Price);
    }

    public async Task CreateBookAsync(BookDto newBook)
    {
        var book = new Book(newBook.Id, newBook.Title, newBook.Author, newBook.Price);

        await bookRepository.AddAsync(book);
        await bookRepository.SaveChangesAsync();
    }

    public async Task DeleteBookAsync(Guid id)
    {
        var book = await bookRepository.GetByIdAsync(id);
        if (book is not null)
        {
            await bookRepository.DeleteAsync(book);
            await bookRepository.SaveChangesAsync();
        }
        //TODO: handle not found case
    }

    public async Task UpdateBookPriceAsync(Guid id, decimal newPrice)
    {
        //TODO: validate price
        var book = await bookRepository.GetByIdAsync(id);
        if (book is not null)
        {
            book.UpdatePrice(newPrice);
            await bookRepository.SaveChangesAsync();
        }
        //TODO: handle not found case
    }
}