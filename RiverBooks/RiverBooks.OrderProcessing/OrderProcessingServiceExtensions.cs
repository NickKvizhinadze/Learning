using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RiverBooks.OrderProcessing.Data;
using RiverBooks.OrderProcessing.Repositories;
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
            opts.UseSqlServer(configuration.GetConnectionString("OrderProcessingDbContextConnectionString")));

        services.AddScoped<IOrderRepository, EfOrderRepository>();
        
        mediatRAssemblies.Add(typeof(OrderProcessingServiceExtensions).Assembly);
        
        logger.Information("{Module} module services registered", "OrderProcessing");

        return services;
    }
}