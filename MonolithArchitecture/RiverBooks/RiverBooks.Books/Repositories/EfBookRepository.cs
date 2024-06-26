using Microsoft.EntityFrameworkCore;
using RiverBooks.Books.Data;
using RiverBooks.Books.Entities;

namespace RiverBooks.Books.Repositories;

internal class EfBookRepository(BookDbContext dbContext) : IBookRepository
{
    public async Task<Book?> GetByIdAsync(Guid id)
    {
        return await dbContext.Books.FindAsync(id);
    }

    public Task<List<Book>> ListAsync()
    {
        return dbContext.Books.ToListAsync();
    }

    public Task AddAsync(Book book)
    {
        dbContext.Books.AddAsync(book);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Book book)
    {
        dbContext.Books.Remove(book);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync()
    {
        return dbContext.SaveChangesAsync();
    }
}