// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Moves.Api.Sdk.Consumer;
using Movies.Api.Sdk;
using Movies.Contracts.Requests;
using Refit;


var services = new ServiceCollection();
services
    .AddHttpClient()
    .AddSingleton<AuthTokenProvider>()
    .AddRefitClient<IMoviesApi>(s => new RefitSettings
    {
        AuthorizationHeaderValueGetter = async (httpRequest, cancellationToken) =>
            await s.GetRequiredService<AuthTokenProvider>().GetTokenAsync()
    })
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7027"));

var provider = services.BuildServiceProvider();

var moviesApi = provider.GetRequiredService<IMoviesApi>();

// var movie = await restService.GetMovieAsync("waiting-to-exhale-1995");
var moviesRequest = new GetAllMoviesRequest
{
    Title = null,
    Year = null,
    SortBy = null,
    Page = 1,
    PageSize = 10
};

var movies = await moviesApi.GetMoviesAsync(moviesRequest);

Console.WriteLine(JsonSerializer.Serialize(movies));