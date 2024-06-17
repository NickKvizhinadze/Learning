using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using RiverBooks.Users.Data;
using RiverBooks.Users.Entities;
using RiverBooks.Users.Repositories;

namespace RiverBooks.Users;

public static class UsersServiceExtensions
{
    public static IServiceCollection AddUserService(
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
        
        mediatRAssemblies.Add(typeof(UsersServiceExtensions).Assembly);
        
        logger.Information("{Module} module services registered", "Users");

        return services;
    }
}