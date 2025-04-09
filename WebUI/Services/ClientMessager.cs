using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using Application.Common.InterFaces.Messager;
using Application.Common.Messager.Enums;
using Application.Common.Messager.Entities;
using Microsoft.Extensions.Options;
using Application.Common.BaseEntities;

namespace WebUI.Services
{
    public class RabbitMQClientMessager : IClientMessager, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IChannel _channel;
        private readonly ConnectionFactory _factory;
        private readonly RabbitMqConfiguration _rabbitMqConfig;





        public RabbitMQClientMessager(IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            _rabbitMqConfig = rabbitMqOptions.Value;
            _factory = new ConnectionFactory
            {
                HostName = _rabbitMqConfig.HostName,
                UserName = _rabbitMqConfig.Username,
                Password = _rabbitMqConfig.Password,
                VirtualHost = _rabbitMqConfig.VirtualHost,
                Port=_rabbitMqConfig.Port,
                AutomaticRecoveryEnabled = _rabbitMqConfig.AutomaticRecoveryEnabled,
                RequestedHeartbeat = TimeSpan.FromSeconds(_rabbitMqConfig.RequestedHeartbeat)
            };

            _connection = _factory.CreateConnectionAsync().GetAwaiter().GetResult();
            _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
        }

        public async Task Publish(MicroServiceName serviceName, string message, string methodName, Guid correlationId, bool isResponseRequired = true)
        {
            var properties = new BasicProperties
            {
                CorrelationId = correlationId.ToString(),
                Type = methodName
            };

            if (isResponseRequired)
            {
                var replyQueueName = $"{serviceName}_reply";
                properties.ReplyTo = replyQueueName;
            }

            var body = Encoding.UTF8.GetBytes(message);
            await _channel.BasicPublishAsync(exchange: "", routingKey: serviceName.ToString(), mandatory: false, basicProperties: properties, body: body);
        }

        public async Task Consume(MicroServiceName serviceName, Guid correlationId, Action<string> handleMessage)
        {
            var queueName = $"{serviceName}_reply";

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                if (ea.BasicProperties.CorrelationId == correlationId.ToString())
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    handleMessage(message);
                    await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                }
                else
                {
                    await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
                }
            };

            await _channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);
        }

        public async Task<BaseResult_VM<T>> CallMethodDirectly<T>(MicroServiceName serviceName, string message, string methodName, TimeSpan? timeout = null, long pointerId = 0L)
        {
            var correlationId = Guid.NewGuid();
            var tcs = new TaskCompletionSource<BaseResult_VM<T>>();

            await Consume(serviceName, correlationId, response =>
            {
                var result = JsonConvert.DeserializeObject<BaseResult_VM<T>>(response);
                tcs.SetResult(result);
            });

            await Publish(serviceName, message, methodName, correlationId);

            if (await Task.WhenAny(tcs.Task, Task.Delay(timeout ?? TimeSpan.FromSeconds(30))) == tcs.Task)
            {
                return await tcs.Task;
            }
            else
            {
                return new BaseResult_VM<T> { Code = -1000, Message = "سرویس مورد نظر پاسخگو نیست " };
            }
        }

        public void Dispose()
        {
            _channel?.CloseAsync().GetAwaiter().GetResult();
            _channel?.DisposeAsync().GetAwaiter().GetResult();
            _connection?.CloseAsync().GetAwaiter().GetResult();
            _connection?.DisposeAsync().GetAwaiter().GetResult();
        }
    }

}
