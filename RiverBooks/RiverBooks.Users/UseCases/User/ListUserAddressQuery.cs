using Ardalis.Result;
using MediatR;
using RiverBooks.Users.Models;

namespace RiverBooks.Users.UseCases.User;

public record ListUserAddressQuery(string? EmailAddress) : IRequest<Result<List<UserAddressDto>>>;