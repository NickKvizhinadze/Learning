using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orders.Data;
using Orders.Domain;
using Orders.Service;
using OrdersApi.Infrastructure.Mappings;
using OrdersApi.Services;

namespace OrderCreation.Worker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<OrderContext>(options =>
                    {
                        options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection"));
                        options.EnableSensitiveDataLogging(true);

                    });

                    services.AddScoped<IOrderRepository, OrderRepository>();
                    services.AddScoped<IOrderService, OrderService>();
                    services.AddAutoMapper(typeof(OrderProfileMapping));

                    
                    services.AddMassTransit(x =>
                    {
                        var entryAssembly = Assembly.GetEntryAssembly();

                        x.AddConsumers(entryAssembly);
                        x.AddActivities(entryAssembly);

                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.ReceiveEndpoint("order-create-command", e =>
                            {
                                e.ConfigureConsumer<OrderCreateConsumer>(context);
                            });
                            
                            cfg.ConfigureEndpoints(context);
                        });
                    });
                });
    }
}