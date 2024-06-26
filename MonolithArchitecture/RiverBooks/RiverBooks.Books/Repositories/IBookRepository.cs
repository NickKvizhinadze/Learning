using RiverBooks.Books.Entities;

namespace RiverBooks.Books.Repositories;

internal interface IBookRepository : IReadOnlyBookRepository
{
    Task AddAsync(Book book);
    Task DeleteAsync(Book book);
    Task SaveChangesAsync();
}