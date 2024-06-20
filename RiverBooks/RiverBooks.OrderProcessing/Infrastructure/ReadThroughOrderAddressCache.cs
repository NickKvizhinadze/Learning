using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using RiverBooks.OrderProcessing.Domain;
using RiverBooks.OrderProcessing.Interfaces;
using RiverBooks.OrderProcessing.Models;

namespace RiverBooks.OrderProcessing.Infrastructure;

internal class ReadThroughOrderAddressCache(
    RedisOrderAddressCache redisCache,
    IMediator mediator,
    ILogger<ReadThroughOrderAddressCache> logger) : IOrderAddressCache
{
    public async Task<Result<OrderAddress>> GetByIdAsync(Guid addressId)
    {
        var result = await redisCache.GetByIdAsync(addressId);

        if (result.IsSuccess)
            return result;

        logger.LogWarning("Address {id} not found; fetching from source", addressId);

        var query = new Users.Contracts.UserAddressDetailsByIdQuery(addressId);

        var queryResult = await mediator.Send(query);
        if (queryResult.IsSuccess)
        {
            var dto = queryResult.Value;
            var address = new Address(
                dto.Street1,
                dto.Street2,
                dto.City,
                dto.State,
                dto.PostalCode,
                dto.Country);

            var orderAddress = new OrderAddress(dto.AddressId, address);

            await StoreAsync(orderAddress);
            return orderAddress;
        }

        return Result.NotFound();
    }
    
    public Task<Result> StoreAsync(OrderAddress orderAddress)
    {
        return redisCache.StoreAsync(orderAddress);
    }
}