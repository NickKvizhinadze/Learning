using System.Reflection;
using ErrorOr;
using MediatR;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Application.Common.Authorization;

namespace GymManagement.Application.Common.Behaviors;

public class AuthorizationBehavior<TRequest, TResponse>(
    ICurrentUserProvider _currentUserProvider)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var authorizeAttributes = request.GetType()
            .GetCustomAttributes<AuthorizeAttribute>()
            .ToList();

        if (authorizeAttributes.Count == 0)
            return await next();

        var requiredPermissions = authorizeAttributes
            .SelectMany(a => a.Permissions?.Split(",") ?? [])
            .ToList();
        
        var currentUser = _currentUserProvider.GetCurrentUser();
        
        if (requiredPermissions.Except(currentUser.Permissions).Any())
            return (dynamic)Error.Forbidden(description: "User is forbidden tot perform this action");
        
        
        var requiredRoles = authorizeAttributes
            .SelectMany(a => a.Roles?.Split(",") ?? [])
            .ToList();
        
        if (requiredRoles.Except(currentUser.Roles).Any())
            return (dynamic)Error.Forbidden(description: "User is forbidden tot perform this action");
        
        return await next();
    }
}