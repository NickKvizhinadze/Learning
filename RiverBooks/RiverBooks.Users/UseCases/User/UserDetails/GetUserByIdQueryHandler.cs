using Ardalis.Result;
using MediatR;
using RiverBooks.Users.Interfaces;
using RiverBooks.Users.Models;

namespace RiverBooks.Users.UseCases.User.UserDetails;

internal class GetUserByIdQueryHandler(IApplicationUserRepository repository) : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await repository.GetUserByIdAsync(request.UserId);
        if (user is null)
            return Result.NotFound();

        return Result<UserDto>.Success(new UserDto(
            user.Id,
            user.Email!,
            user.FullName));
    }
}