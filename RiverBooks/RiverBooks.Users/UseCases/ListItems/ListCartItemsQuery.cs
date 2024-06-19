using Ardalis.Result;
using MediatR;
using RiverBooks.Users.Models;

namespace RiverBooks.Users.UseCases.ListItems;

public record ListCartItemsQuery(string? EmailAddress) : IRequest<Result<List<CartItemDto>>>;

