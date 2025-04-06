using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.BaseEntites.Common;
using Application.Common.Messager.Enums;

namespace Application.Common.InterFaces.Messager
{
    public interface IClientMessager
    {
        Task Publish(MicroServiceName serviceName, string message, string methodName, Guid correlationId, bool isResponseRequired = true);

        Task Consume(MicroServiceName serviceName, Guid correlationId, Action<string> handleMessage);

        Task<BaseResult_VM<T>> CallMethodDirectly<T>(MicroServiceName serviceName, string message, string methodName, TimeSpan? timeout = null, long pointerId = 0L);
    }
}
