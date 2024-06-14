using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Dometrain.EFCore.API.Data;
using Dometrain.EfCore.API.Repositories;
using Dometrain.EFCore.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});


// Configure Serilog
var serilog = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

// Configure it for Microsoft.Extensions.Logging
builder.Services.AddSerilog(serilog);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IGenreRepository, GenreRepository>();
builder.Services.AddTransient<IBatchGenreService, BatchGenreService>();
builder.Services.AddScoped<IUnitOfWorkManager, UnitOfWorkManager>();

// Add a DbContext here

builder.Services.AddDbContext<MoviesContext>(options =>
{
    options
    .UseSqlServer(
        builder.Configuration.GetConnectionString("MoviesContext"),
        sqlBuilder => sqlBuilder.MaxBatchSize(50));
},
ServiceLifetime.Scoped,
ServiceLifetime.Singleton);

var app = builder.Build();

//Dirty hack, clean up later
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MoviesContext>();
    var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
    if (pendingMigrations.Count() > 0)
        throw new Exception("There are pending migrations");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();