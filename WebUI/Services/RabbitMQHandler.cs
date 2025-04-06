//using Application.Common.InterFaces.Messager;
//using Application.Common.Messager.Entities;
//using Application.Common.Messager.Enums;
//using Microsoft.Azure.WebJobs;

//namespace WebUI.Services
//{
//    [Singleton]
//    public class RabbitMQHandler : BaseRabbitMQHandler
//    {
//        public RabbitMQHandler(IServiceProvider serviceProvider, IExceptionHandler exeptionHandler, IServerMessager serverMessager) : base(serviceProvider, exeptionHandler, serverMessager)
//        {
//        }

//        public override async Task Consume()
//        {
//            await Task.CompletedTask;
//            await ConsumeToQueue(new RabbitMQConsumeRequest
//            {
//                MicroServiceName = MicroServiceName.Tax,
//                ListenerList = new List<ListenerClass>
//                        {
//                            new ListenerClass<GetPackageListQuery , BaseResult_VM<object>>(){MethodName = "Package" + "_" + nameof(PackageController.GetList)},
//                        }
//            });
//        }
//    }
//}
