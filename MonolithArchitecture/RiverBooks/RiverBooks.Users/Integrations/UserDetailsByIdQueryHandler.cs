using Ardalis.Result;
using MediatR;
using RiverBooks.Users.Contracts;
using RiverBooks.Users.UseCases.User.UserDetails;

namespace RiverBooks.Users.Integrations;

internal class UserDetailsByIdQueryHandler(IMediator mediator)
    : IRequestHandler<UserDetailsByIdQuery, Result<UserDetails>>
{
    public async Task<Result<UserDetails>> Handle(UserDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        var getUserByIdQuery = new GetUserByIdQuery(request.UserId);
        var userResult = await mediator.Send(getUserByIdQuery, cancellationToken);

        return userResult.Map(u => new UserDetails(u.UserId, u.Email, u.FullName));
    }
}