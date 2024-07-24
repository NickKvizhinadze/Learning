using Movies.Api.Auth;
using Movies.Application.Services;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Endpoints.Ratings;

public static class RateMovieEndpoint
{
    public const string Name = "RateMovies";

    public static IEndpointRouteBuilder MapRateMovie(this IEndpointRouteBuilder app)
    {
        app.MapPut(ApiEndpoints.Movies.Rate, async (
                Guid id,
                RateMovieRequest request,
                IRatingService ratingService,
                HttpContext context,
                CancellationToken token) =>
            {
                var userId = context.GetUserId();
                var result = await ratingService.RateMovieAsync(id, request.Rating, userId!.Value, token);

                return result 
                    ? Results.Ok() 
                    : Results.NotFound();
            })
            .WithName(Name)
            .Produces<IEnumerable<MovieRatingResponse>>(StatusCodes.Status200OK)
            .RequireAuthorization();

        return app;
    }
}