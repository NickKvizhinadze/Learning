using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;

namespace Movies.Application.Services;

public class MovieService(IMoviesRepository _repository, IRatingRepository _ratingRepository, IValidator<Movie> _validator) : IMovieService
{
    public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default)
    {
        await _validator.ValidateAndThrowAsync(movie, cancellationToken: token);
        return await _repository.CreateAsync(movie, token);
    }

    public Task<Movie?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken token = default)
        => _repository.GetByIdAsync(id, userId, token);

    public Task<Movie?> GetBySlugAsync(string slug, Guid? userId = default, CancellationToken token = default)
        => _repository.GetBySlugAsync(slug, userId, token);

    public Task<IEnumerable<Movie>> GetAllAsync(GetAllMoviesOptions options, CancellationToken token = default)
        => _repository.GetAllAsync(options, token);

    public async Task<Movie?> UpdateAsync(Movie movie, Guid? userId = default, CancellationToken token = default)
    {
        await _validator.ValidateAndThrowAsync(movie, cancellationToken: token);
        var movieExists = await _repository.ExistsByIdAsync(movie.Id, token);
        if (!movieExists)
            return null;

        var result = await _repository.UpdateAsync(movie, token);

        if (!userId.HasValue)
            movie.Rating = await _ratingRepository.GetRatingAsync(movie.Id, token);
        else
        {
            var (rating, userRating) =  await _ratingRepository.GetRatingAsync(movie.Id, userId, token);
            movie.Rating = rating;
            movie.UserRating = userRating;
        }

        return result ? movie : null;
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default) 
        => _repository.DeleteByIdAsync(id, token);
}