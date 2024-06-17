using Ardalis.Result;
using MediatR;
using RiverBooks.Users.Models;

namespace RiverBooks.Users.UseCases;

public record ListCartItemsQuery(string? EmailAddress) : IRequest<Result<List<CartItemDto>>>;

