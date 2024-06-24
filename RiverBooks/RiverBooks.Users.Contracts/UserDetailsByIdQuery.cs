using Ardalis.Result;
using MediatR;

namespace RiverBooks.Users.Contracts;

public record UserDetailsByIdQuery(string UserId) :IRequest<Result<UserDetails>>;