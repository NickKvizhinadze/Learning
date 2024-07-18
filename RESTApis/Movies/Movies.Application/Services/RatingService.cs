﻿using FluentValidation;
using FluentValidation.Results;
using Movies.Application.Models;
using Movies.Application.Repositories;

namespace Movies.Application.Services;

public class RatingService(IRatingRepository _repository, IMoviesRepository _moviesRepository) : IRatingService
{
    public async Task<bool> RateMovieAsync(Guid movieId, int rating, Guid userId, CancellationToken token = default)
    {
        if (rating is <= 0 or > 5)
            throw new ValidationException(
                new[]
                {
                    new ValidationFailure
                    {
                        PropertyName = "Rating",
                        ErrorMessage = "Rating must be between 1 and 5"
                    }
                });

        var movieExists = await _moviesRepository.ExistsByIdAsync(movieId, token);
        if (!movieExists)
            return false;

        return await _repository.RateMovieAsync(movieId, rating, userId, token);
    }

    public Task<bool> DeleteRatingAsync(Guid movieId, Guid userId, CancellationToken token = default)
        => _repository.DeleteRatingAsync(movieId, userId, token);

    public Task<IEnumerable<MovieRating>> GetRatingsForUserAsync(Guid userId, CancellationToken token = default)
        => _repository.GetRatingsForUserAsync(userId, token);
}