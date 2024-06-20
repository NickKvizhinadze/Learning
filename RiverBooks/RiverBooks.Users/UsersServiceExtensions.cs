using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using RiverBooks.Users.Domain;
using RiverBooks.Users.Infrastructure.Data;
using RiverBooks.Users.Interfaces;

namespace RiverBooks.Users;

public static class UsersServiceExtensions
{
    public static IServiceCollection AddUserModuleService(
        this IServiceCollection services,
        ConfigurationManager configuration,
        ILogger logger,
        List<Assembly> mediatRAssemblies)
    {
        services.AddDbContext<UsersDbContext>(opts =>
            opts.UseSqlServer(configuration.GetConnectionString("UsersConnectionString")));

        services.AddIdentityCore<ApplicationUser>()
            .AddEntityFrameworkStores<UsersDbContext>();

        services.AddScoped<IApplicationUserRepository, EfApplicationUserRepository>();
        services.AddScoped<IReadOnlyUsersStreetAddressRepository, EfUsersStreetAddressRepository>();
        
        mediatRAssemblies.Add(typeof(UsersServiceExtensions).Assembly);
        
        logger.Information("{Module} module services registered", "Users");

        return services;
    }
}