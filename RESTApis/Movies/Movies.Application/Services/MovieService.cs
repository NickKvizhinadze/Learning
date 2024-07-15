using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;

namespace Movies.Application.Services;

public class MovieService(IMoviesRepository _repository, IValidator<Movie> _validator) : IMovieService
{
    public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default)
    {
        await _validator.ValidateAndThrowAsync(movie, cancellationToken: token);
        return await _repository.CreateAsync(movie, token);
    }

    public Task<Movie?> GetByIdAsync(Guid id, CancellationToken token = default)
        => _repository.GetByIdAsync(id, token);

    public Task<Movie?> GetBySlugAsync(string slug, CancellationToken token = default)
        => _repository.GetBySlugAsync(slug, token);

    public Task<IEnumerable<Movie>> GetAllAsync(CancellationToken token = default)
        => _repository.GetAllAsync(token);

    public async Task<Movie?> UpdateAsync(Movie movie, CancellationToken token = default)
    {
        await _validator.ValidateAndThrowAsync(movie, cancellationToken: token);
        var movieExists = await _repository.ExistsByIdAsync(movie.Id, token);
        if (!movieExists)
            return null;

        var result = await _repository.UpdateAsync(movie, token);

        return result ? movie : null;
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default) 
        => _repository.DeleteByIdAsync(id, token);
}