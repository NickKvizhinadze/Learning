using System.Data;
using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;

namespace Movies.Application.Validators;

public class MovieValidator: AbstractValidator<Movie>
{
    private readonly IMoviesRepository _repository;
    
    public MovieValidator(IMoviesRepository repository)
    {
        _repository = repository;
        RuleFor(m => m.Id)
            .NotEmpty();

        RuleFor(m => m.Genres)
            .NotEmpty();

        RuleFor(m => m.Title)
            .NotEmpty();

        RuleFor(m => m.YearOfRelease)
            .LessThanOrEqualTo(DateTime.UtcNow.Year);
        
        RuleFor(m => m.Slug)
            .MustAsync(ValidateSlug)
            .WithMessage("This movie already exists");
    }
    
    private async Task<bool> ValidateSlug(Movie moive, string slug, CancellationToken cancellationToken)
    {
        var existingMovie = await _repository.GetBySlugAsync(slug, null, cancellationToken);
        if(existingMovie is not null)
            return existingMovie.Id == moive.Id;
        
        return existingMovie is null;
    }
}