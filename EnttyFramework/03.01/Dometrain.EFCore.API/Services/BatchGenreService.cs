using Dometrain.EFCore.API.Data;
using Dometrain.EFCore.API.Models;
using Dometrain.EfCore.API.Repositories;

namespace Dometrain.EFCore.API.Services
{
    public interface IBatchGenreService
    {
        Task<IEnumerable<Genre>> CreateGenres(IEnumerable<Genre> genres);
        Task<object?> UpdateGenres(List<Genre> genres);
    }

    public class BatchGenreService(IGenreRepository genreRepository, IUnitOfWorkManager uowManager) : IBatchGenreService
    {

        public async Task<IEnumerable<Genre>> CreateGenres(IEnumerable<Genre> genres)
        {
            uowManager.StartUnitOfWork();
            List<Genre> response = new();
            foreach (var genre in genres)
            {
                response.Add(await genreRepository.Create(genre));
            }

            await uowManager.SaveChangesAsync();
            return response;
        }

        public async Task<object?> UpdateGenres(List<Genre> genres)
        {
            List<Genre> response = new();
            uowManager.StartUnitOfWork();
            
            var existingGenres = await genreRepository.GetAll(genres.Select(g => g.Id));
            
            foreach (var genre in genres)
            {
                var updatedGenre = await genreRepository.Update(genre.Id, genre);
                if(updatedGenre is not null)
                    response.Add(updatedGenre);
            }
            
            await uowManager.SaveChangesAsync();
            return response;
        }
    }
}
