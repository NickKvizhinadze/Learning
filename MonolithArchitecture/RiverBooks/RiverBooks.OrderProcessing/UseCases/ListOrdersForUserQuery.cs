using Ardalis.Result;
using MediatR;
using RiverBooks.OrderProcessing.Models;

namespace RiverBooks.OrderProcessing.UseCases;

public record ListOrdersForUserQuery(string? EmailAddress) : IRequest<Result<List<OrderSummary>>>;

