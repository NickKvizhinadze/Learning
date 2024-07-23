using Movies.Contracts.Requests;
using Refit;
using Movies.Contracts.Responses;

namespace Movies.Api.Sdk;

[Headers("Authorization: Bearer")]
public interface IMoviesApi
{
    [Get(ApiEndpoints.Movies.Get)]
    Task<MovieResponse> GetMovieAsync(string idOrSlug);
    
    [Get(ApiEndpoints.Movies.GetAll)]
    Task<MoviesResponse> GetMoviesAsync(GetAllMoviesRequest request);
    
    [Post(ApiEndpoints.Movies.Create)]
    Task<MovieResponse> GetMoviesAsync(CreateMovieRequest request);
    
    [Put(ApiEndpoints.Movies.Update)]
    Task<MovieResponse> UpdateMoviesAsync(Guid id, UpdateMovieRequest request);
    
    [Delete(ApiEndpoints.Movies.Delete)]
    Task DeleteMoviesAsync(Guid id);
    
    [Put(ApiEndpoints.Movies.Rate)]
    Task RateMoviesAsync(Guid id, RateMovieRequest request);
    
    [Delete(ApiEndpoints.Movies.DeleteRating)]
    Task DeleteMovieRatingAsync(Guid id);
    
    [Get(ApiEndpoints.Ratings.GetUserRatings)]
    Task<IEnumerable<MovieRatingResponse>> GetUserRatingAsync();
}