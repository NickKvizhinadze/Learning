using System.Diagnostics;
using System.Text;
using System.Text.Json;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using RabbitMQ.Client;

namespace Infrastructure.RabbitMQ;

public class RabbitMqPublisher
{
    private readonly ConnectionFactory _factory;

    public RabbitMqPublisher(string connectionString)
    {
        _factory = new ConnectionFactory()
        {
            Uri = new Uri(connectionString)
        };
    }

    public void Publish<T>(T @event, string exchange)
    {
        using var connection = _factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Fanout, durable: true);

        var message = JsonSerializer.Serialize(@event);
        var body = Encoding.UTF8.GetBytes(message);

        const string operation = "publish";
        var eventType = @event!.GetType().Name;

        var activityName = $"{eventType} {operation}";

        using var activity = RabbitMqDiagnostics.ActivitySource.StartActivity(activityName);

        ActivityContext contextToInject = default;
        if (activity != null)
            contextToInject = activity.Context;
        else if (Activity.Current != null)
            contextToInject = Activity.Current.Context;

        var properties = channel.CreateBasicProperties();
        properties.DeliveryMode = 2;
        
        RabbitMqDiagnostics.Propagator.Inject(
            new PropagationContext(contextToInject, Baggage.Current),
            properties,
            InjectContextIntoBasicProperties);

        SetActivityContext(activity, eventType, operation);
        
        channel.BasicPublish(exchange: exchange, routingKey: string.Empty, basicProperties: properties, body: body);
    }

    private void SetActivityContext(Activity? activity, string eventType, string operation)
    {
        if (activity is null)
            return;

        activity.SetTag("messaging.system", "rabbitmq");
        activity.SetTag("messaging.destination_kind", "exchange");
        activity.SetTag("messaging.operation", operation);
        activity.SetTag("messaging.destination.name", eventType);
    }

    private void InjectContextIntoBasicProperties(IBasicProperties props, string key, string value)
    {
        props.Headers ??= new Dictionary<string, object>();
        props.Headers[key] = value;
    }
}