using System.Reflection;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Serilog;
using RiverBooks.Books;
using RiverBooks.Users;

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
List<Assembly> mediatRAssemblies = [typeof(Program).Assembly];
builder.Services.AddBookService(builder.Configuration, logger, mediatRAssemblies);
builder.Services.AddUserService(builder.Configuration, logger, mediatRAssemblies);

// Setup MediatR
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssemblies(mediatRAssemblies.ToArray()));

var app = builder.Build();
app.UseHttpsRedirection();

app.UseAuthentication()
    .UseAuthorization();

app.UseFastEndpoints()
    .UseSwaggerGen();

app.Run();

// Need for testing
public partial class Program
{
}
