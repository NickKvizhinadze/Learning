using System.Data;
using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;

namespace Movies.Application.Validators;

public class GetAllMoviesOptionsValidator: AbstractValidator<GetAllMoviesOptions>
{
    private readonly IMoviesRepository _repository;
    
    public GetAllMoviesOptionsValidator(IMoviesRepository repository)
    {
        _repository = repository;
        RuleFor(m => m.Year)
            .LessThanOrEqualTo(DateTime.UtcNow.Year);
    }
}