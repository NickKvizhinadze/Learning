using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RiverBooks.Reporting.Infrastructure;
using RiverBooks.Reporting.Interfaces;
using Serilog;

namespace RiverBooks.Reporting;

public static class ReportingServiceExtensions
{
    public static IServiceCollection AddReportingModuleService(
        this IServiceCollection services,
        ConfigurationManager configuration,
        ILogger logger,
        List<Assembly> mediatRAssemblies)
    {
        services.AddScoped<ITopSellingBooksReportService, TopSellingBooksReportServiceService>();
        services.AddScoped<IOrderIngestionService, OrderIngestionService>();
        services.AddScoped<ISalesReportingService, DefaultSalesReportingService>();
        
        mediatRAssemblies.Add(typeof(ReportingServiceExtensions).Assembly);
        
        logger.Information("{Module} module services registered", "Reporting");

        return services;
    }
}