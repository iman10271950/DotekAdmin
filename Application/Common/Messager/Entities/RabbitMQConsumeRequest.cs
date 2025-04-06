using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Messager.Enums;

namespace Application.Common.Messager.Entities
{
    public class RabbitMQConsumeRequest
    {
        public MicroServiceName MicroServiceName { get; set; }

        public List<ListenerClass> ListenerList { get; set; }
    }
}
