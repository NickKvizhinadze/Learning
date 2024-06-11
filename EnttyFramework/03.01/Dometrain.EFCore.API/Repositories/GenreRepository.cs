using Dometrain.EFCore.API.Data;
using Dometrain.EFCore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Dometrain.EfCore.API.Repositories;

public interface IGenreRepository
{
    Task<IEnumerable<Genre>> GetAll();
    Task<Genre?> Get(int id);
    Task<Genre> Create(Genre genre);
    Task<Genre?> Update(int id, Genre genre);
    Task<bool> Delete(int id);
}

public class GenreRepository(MoviesContext context, IUnitOfWorkManager unitOfWorkManager)
    : IGenreRepository
{
    public async Task<IEnumerable<Genre>> GetAll()
    {
        return await context.Genres.ToListAsync();
    }

    public async Task<Genre?> Get(int id)
    {
        return await context.Genres.FindAsync(id);
    }

    public async Task<Genre> Create(Genre genre)
    {
        await context.Genres.AddAsync(genre);
        
        if(!unitOfWorkManager.IsUnitOfWorkStarted)
            await context.SaveChangesAsync();

        return genre;
    }

    public async Task<Genre?> Update(int id, Genre genre)
    {
        var existingGenre = await context.Genres.FindAsync(id);

        if (existingGenre is null)
            return null;

        existingGenre.Name = genre.Name;

        if(!unitOfWorkManager.IsUnitOfWorkStarted)
            await context.SaveChangesAsync();

        return existingGenre;
    }

    public async Task<bool> Delete(int id)
    {
        var existingGenre = await context.Genres.FindAsync(id);

        if (existingGenre is null)
            return false;

        context.Genres.Remove(existingGenre);

        if(!unitOfWorkManager.IsUnitOfWorkStarted)
            await context.SaveChangesAsync();
        return true;
    }
}