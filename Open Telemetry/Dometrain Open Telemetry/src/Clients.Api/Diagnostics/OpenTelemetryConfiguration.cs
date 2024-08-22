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
        const string serviceName = "Clients.Api";
        var otlpEndpoint = builder.Configuration.GetValue<string>("OTLP_Endpoint");

        builder.Services.ConfigureOpenTelemetryTracerProvider(provider 
            => provider.SetSampler(new RateSampler(0.25)));

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
                .AddGrpcClientInstrumentation()
                .AddHttpClientInstrumentation()
                .AddNpgsql()
                .AddSource(RabbitMqDiagnostics.ActivitySourceName)
                .AddRedisInstrumentation()
                .AddConsoleExporter()
                .AddOtlpExporter(options => options.Endpoint = new Uri(otlpEndpoint!))
            )
            .WithMetrics(metric =>
                metric
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    // Metrics provides by ASP.NET
                    .AddMeter("Microsoft.AspNetCore.Hosting")
                    .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
                    .AddMeter(ApplicationDiagnostics.Meter.Name)
                    .AddConsoleExporter()
                    .AddOtlpExporter(options => options.Endpoint = new Uri(otlpEndpoint!))
            )
            .WithLogging(logging =>
                // logging.AddConsoleExporter()
                logging.AddOtlpExporter(options => { options.Endpoint = new Uri(otlpEndpoint!); })
            );

        return builder;
    }
}