using System.Reflection;
using Infrastructure.RabbitMQ;
using Npgsql;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;

namespace Clients.Api.Diagnostics;

public static class OpenTelemetryConfiguration
{
    public static WebApplicationBuilder AddOpenTelemetry(this WebApplicationBuilder builder)
    {
        const string serviceName = "Accounts";
        var otlpEndpoint = builder.Configuration.GetValue<string>("OTLP_Endpoint");
        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource
                .AddService(serviceName //,
                    // serviceNamespace: "Dometrain.Courses.OpenTelemetry",
                    // serviceVersion:Assembly.GetExecutingAssembly().GetName().Version!.ToString()
                )
                .AddAttributes(new[]
                {
                    new KeyValuePair<string, object>("service.version",
                        Assembly.GetExecutingAssembly().GetName().Version!.ToString())
                })
            )
            .WithTracing(tracing => tracing
                .AddAspNetCoreInstrumentation()
                .AddNpgsql()
                .AddSource(RabbitMqDiagnostics.ActivitySourceName)
                .AddOtlpExporter(options => options.Endpoint = new Uri(otlpEndpoint!))
            )
            .WithMetrics(metric => 
                metric
                    .AddAspNetCoreInstrumentation()
                    // Metrics provides by ASP.NET
                    .AddMeter("Microsoft.AspNetCore.Hosting")
                    .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
                    .AddOtlpExporter(options => options.Endpoint = new Uri(otlpEndpoint!))
                )
            .WithLogging(logging =>
                // logging.AddConsoleExporter()
                logging.AddOtlpExporter(options => { options.Endpoint = new Uri(otlpEndpoint!); })
            );
        
        return builder;
    }
}