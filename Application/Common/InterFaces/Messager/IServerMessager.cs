using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Messager.Enums;

namespace Application.Common.InterFaces.Messager
{
    public interface IServerMessager
    {
        Task Publish(string serviceName, string message, Guid correlationId, string methodName);

        Task Consume(MicroServiceName serviceName, Action<string, string, Guid, string> handleMessage);

        Task Detach();
    }
}
