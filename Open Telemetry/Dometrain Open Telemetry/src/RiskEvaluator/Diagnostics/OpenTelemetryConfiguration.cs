using System.Reflection;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace RiskEvaluator.Diagnostics;

public static class OpenTelemetryConfiguration
{
    public static WebApplicationBuilder AddOpenTelemetry(this WebApplicationBuilder builder)
    {
        const string serviceName = "RiskEvaluator";
        var otlpEndpoint = builder.Configuration.GetValue<string>("OTLP_Endpoint");

        builder.Services.ConfigureOpenTelemetryTracerProvider((provider, providerBuilder) =>
        {
            providerBuilder.AddProcessor(new BaggageProcessor());
        });
        
        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource
                .AddService(serviceName)
                .AddAttributes(new[]
                {
                    new KeyValuePair<string, object>("service.version",
                        Assembly.GetExecutingAssembly().GetName().Version!.ToString())
                })
            )
            .WithTracing(tracing =>
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddSource(ApplicationDiagnostics.ActivitySourceName)
                    // .AddConsoleExporter()
                    .AddOtlpExporter(options => { options.Endpoint = new Uri(otlpEndpoint!); })
            )
            .WithLogging(logging =>
                // logging.AddConsoleExporter()
                logging.AddOtlpExporter(options => { options.Endpoint = new Uri(otlpEndpoint!); })
            );

        return builder;
    }
}