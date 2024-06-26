using Ardalis.Result;
using MediatR;
using RiverBooks.OrderProcessing.Models;

namespace RiverBooks.OrderProcessing.UseCases;

public record CheckoutCommand(string? EmailAddress) : IRequest<Result<List<OrderSummary>>>;

