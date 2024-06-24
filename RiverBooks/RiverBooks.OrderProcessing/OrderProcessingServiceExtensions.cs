using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RiverBooks.OrderProcessing.Data;
using RiverBooks.OrderProcessing.Infrastructure;
using RiverBooks.OrderProcessing.Infrastructure.Data;
using RiverBooks.OrderProcessing.Interfaces;
using Serilog;

namespace RiverBooks.OrderProcessing;

public static class OrderProcessingServiceExtensions
{
    public static IServiceCollection AddOrderModuleService(
        this IServiceCollection services,
        ConfigurationManager configuration,
        ILogger logger,
        List<Assembly> mediatRAssemblies)
    {
        services.AddDbContext<OrderProcessingDbContext>(opts =>
            opts.UseSqlServer(configuration.GetConnectionString("OrderProcessingConnectionString")));

        services.AddScoped<IOrderRepository, EfOrderRepository>();
        services.AddScoped<RedisOrderAddressCache>();
        services.AddScoped<IOrderAddressCache, ReadThroughOrderAddressCache>();
        
        mediatRAssemblies.Add(typeof(OrderProcessingServiceExtensions).Assembly);
        
        logger.Information("{Module} module services registered", "OrderProcessing");

        return services;
    }
}