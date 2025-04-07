using Application.Common.Interfaces.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Logger
{
    public class LoggerPointer : ILoggerPointer
    {
        public LoggerPointer()
        {
            Pointer = false;
        }
        public bool Pointer { get; set; }
    }
}
