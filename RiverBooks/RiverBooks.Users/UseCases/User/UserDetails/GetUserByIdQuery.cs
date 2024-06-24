using Ardalis.Result;
using MediatR;
using RiverBooks.Users.Models;

namespace RiverBooks.Users.UseCases.User.UserDetails;

internal record GetUserByIdQuery(string UserId) : IRequest<Result<UserDto>>;