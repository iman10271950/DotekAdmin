namespace WebUI.Services;
using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Application.Common.InterFaces.Messager;
using Application.Common.Messager.Enums;

public class RabbitMQServerMessager : IServerMessager, IDisposable
{
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly ConnectionFactory _factory;

    public RabbitMQServerMessager()
    {
        _factory = new ConnectionFactory { HostName = "localhost" };
       
    }

    public async Task Publish(string serviceName, string message, Guid correlationId, string methodName)
    {
        EnsureConnectionAndChannel();

        var body = Encoding.UTF8.GetBytes(message);

        var properties = new BasicProperties
        {
            CorrelationId = correlationId.ToString(),
            Type = methodName
        };

        await _channel!.BasicPublishAsync(
            exchange: "",
            routingKey: serviceName,
            mandatory: false,
            basicProperties: properties,
            body: body
        );
    }

    public async Task Consume(MicroServiceName serviceName, Action<string, string, Guid, string> handleMessage)
    {
        EnsureConnectionAndChannel();

        string queueName = serviceName.ToString();

        await _channel!.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            string message = Encoding.UTF8.GetString(body);
            string methodName = ea.BasicProperties.Type ?? string.Empty;
            string correlationIdStr = ea.BasicProperties.CorrelationId ?? Guid.NewGuid().ToString();
            string replyTo = ea.BasicProperties.ReplyTo ?? string.Empty;

            if (Guid.TryParse(correlationIdStr, out Guid correlationId))
            {
                handleMessage(message, methodName, correlationId, replyTo);
            }

            await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
        };

        await _channel.BasicConsumeAsync(
            queue: queueName,
            autoAck: false,
            consumer: consumer
        );
    }

    public async Task Detach()
    {
        if (_channel != null)
        {
            await _channel.CloseAsync();
            _channel.Dispose();
            _channel = null;
        }

        if (_connection != null)
        {
            await _connection.CloseAsync();
            _connection.Dispose();
            _connection = null;
        }
    }

    private void EnsureConnectionAndChannel()
    {
        if (_connection == null || !_connection.IsOpen)
        {
            _connection = _factory.CreateConnectionAsync().GetAwaiter().GetResult();
        }

        if (_channel == null || !_channel.IsOpen)
        {
            _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
        }
    }

    public void Dispose()
    {
        Detach().GetAwaiter().GetResult();
    }
}





