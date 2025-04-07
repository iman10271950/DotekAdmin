
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.BaseEntities;
using Application.Common.InterFaces.Messager;
using Application.Common.Messager.Enums;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;


namespace Application.Common.Messager.Entities;

public abstract class BaseRabbitMQHandler : IHostedService
{
    public IServiceProvider ServiceProvider { get; }

    public IExceptionHandler ExeptionHandler { get; }

    public IServerMessager ServerMessager { get; }

    private RabbitMQConsumeRequest rabbitMQConsume { get; set; }

    public BaseRabbitMQHandler(IServiceProvider serviceProvider, IExceptionHandler exeptionHandler, IServerMessager serverMessager)
    {
        ServiceProvider = serviceProvider;
        ExeptionHandler = exeptionHandler;
        ServerMessager = serverMessager;
    }

    public async Task ConsumeToQueue(RabbitMQConsumeRequest request)
    {
        rabbitMQConsume = request;
        await ServerMessager.Consume(request.MicroServiceName, HandleRequests);
    }

    private async void HandleRequests(string message, string methodName, Guid correlationId, string replayTo)
    {
        string methodName2 = methodName;
        try
        {
            ListenerClass requestClass = rabbitMQConsume.ListenerList.FirstOrDefault((ListenerClass x) => x.MethodName == methodName2);
            if (requestClass == null)
            {
                BaseResult_VM<object> returnResponse2 = new BaseResult_VM<object>(null, -100, "Method Name Is Not Right.");
                await ServerMessager.Publish(replayTo, JsonConvert.SerializeObject(returnResponse2), correlationId, methodName2);
                return;
            }

            Type genericType = requestClass.GetType();
            Type[] typeArguments = genericType.GetGenericArguments();
            Type TRequest = typeArguments[0];
            Type TResponse = typeArguments[1];
            MethodInfo method = typeof(BaseRabbitMQHandler).GetMethod("InvokeMethod", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            MethodInfo generic = method.MakeGenericMethod(TRequest, TResponse);
            object[] param = new object[5] { message, correlationId, methodName2, rabbitMQConsume.MicroServiceName, replayTo };
            Task salamMethod = (Task)generic.Invoke(this, param);
            await salamMethod;
        }
        catch (Exception ex)
        {
            BaseResult_VM<object> returnResponse = ExeptionHandler.HandleException(ex);
            returnResponse.Result = ex;
            try
            {
                rabbitMQConsume.ListenerList.FirstOrDefault((ListenerClass x) => x.MethodName == methodName2);
                await ServerMessager.Publish(replayTo, JsonConvert.SerializeObject(returnResponse), correlationId, methodName2);
            }
            catch
            {
            }
        }
    }

    private async Task InvokeMethod<T, Y>(string message, Guid correlationId, string methodName, MicroServiceName microServiceName, string replayTo) where T : IRequest<Y>
    {
        T query = JsonConvert.DeserializeObject<T>(message);
        if (query != null)
        {
            using IServiceScope scope = ServiceProvider.CreateScope();
            IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            BaseResult_VM<Y> returnResponse2 = new BaseResult_VM<Y>(await mediator.Send((IRequest<Y>)query, default(CancellationToken)), 0, "Ok", success: true);
            await ServerMessager.Publish(replayTo, JsonConvert.SerializeObject(returnResponse2), correlationId, methodName);
        }
        else
        {
            BaseResult_VM<Y> returnResponse = new BaseResult_VM<Y>(default(Y), -200, "Query Class Is Not In The Right Format");
            await ServerMessager.Publish(replayTo, JsonConvert.SerializeObject(returnResponse), correlationId, methodName);
        }
    }

    public abstract Task Consume();

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await Consume();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await ServerMessager.Detach();
    }
}