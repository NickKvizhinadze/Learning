using FluentValidation;
using ErrorOr;
using MediatR;

namespace GymManagement.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly IValidator<TRequest>? _validator;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>>? validators)
    {
        _validator = validators?.FirstOrDefault();
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validator is null)
            return await next();
        
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid)
            return await next();

        var errors = validationResult.Errors
            .ConvertAll(error => Error.Validation(code: error.PropertyName,
                description: error.ErrorMessage))
            .ToList();

        return (dynamic)errors;
    }
}