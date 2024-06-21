using System.Reflection;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using RiverBooks.Books;
using RiverBooks.OrderProcessing;
using RiverBooks.SharedKernel;
using RiverBooks.Users;
using RiverBooks.Users.UseCases.AddItem;
using Serilog;

var logger = Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

logger.Information("Starting web host");

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, config) => config
    .ReadFrom.Configuration(builder.Configuration));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddFastEndpoints()
    .AddAuthenticationJwtBearer(o =>
        {
            o.SigningKey = builder.Configuration["Auth:JwtSecret"];
        })
    .AddAuthorization()
    .SwaggerDocument();

// Add module services
List<Assembly> mediatRAssemblies = [typeof(RiverBooks.Web.Program).Assembly];
builder.Services.AddBookModuleService(builder.Configuration, logger, mediatRAssemblies);
builder.Services.AddUserModuleService(builder.Configuration, logger, mediatRAssemblies);
builder.Services.AddOrderModuleService(builder.Configuration, logger, mediatRAssemblies);

// Setup MediatR
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssemblies(mediatRAssemblies.ToArray()));
builder.Services.AddMediatRLoggingBehaviors();
builder.Services.AddMediatRValidationBehaviors();
builder.Services.AddValidatorsFromAssemblyContaining<AddItemToCartCommandValidator>();

builder.Services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();

var app = builder.Build();
app.UseHttpsRedirection();

app.UseAuthentication()
    .UseAuthorization();

app.UseFastEndpoints()
    .UseSwaggerGen();

app.Run();

// Need for testing
namespace RiverBooks.Web
{
    public partial class Program
    {
    }
}
