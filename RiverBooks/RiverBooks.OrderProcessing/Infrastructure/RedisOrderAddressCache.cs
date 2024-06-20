using System.Text.Json;
using Ardalis.Result;
using Microsoft.Extensions.Logging;
using RiverBooks.OrderProcessing.Interfaces;
using RiverBooks.OrderProcessing.Models;
using StackExchange.Redis;

namespace RiverBooks.OrderProcessing.Infrastructure;

internal class RedisOrderAddressCache: IOrderAddressCache
{
    private readonly IDatabase _db;
    private readonly ILogger<RedisOrderAddressCache> _logger;

    public RedisOrderAddressCache(ILogger<RedisOrderAddressCache> logger)
    {
        var redis = ConnectionMultiplexer.Connect("localhost");
        _db = redis.GetDatabase();
        _logger = logger;
    }
    
    public async Task<Result<OrderAddress>> GetByIdAsync(Guid addressId)
    {
        string? fetchedJson = await _db.StringGetAsync(addressId.ToString());
        
        if(fetchedJson is null)
        {
            _logger.LogWarning("Address not found in {db} for id {AddressId}", addressId, "Redis");
            return Result<OrderAddress>.NotFound();
        }
        
        var address = JsonSerializer.Deserialize<OrderAddress>(fetchedJson);
        if(address is null)
            return Result<OrderAddress>.NotFound();
        
        _logger.LogInformation("Address {id} returned from {db}", addressId, "Redis");
        return Result.Success(address);
    }

    public async Task<Result> StoreAsync(OrderAddress orderAddress)
    {
        var json = JsonSerializer.Serialize(orderAddress);
        await _db.StringSetAsync(orderAddress.Id.ToString(), json);
        
        _logger.LogInformation("Address {id} stored in {db}", orderAddress.Id, "Redis");
        return Result.Success();
    }
}