using Domain.Enums.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Auth
{
    public interface IExecutionMode
    {
        public ExecutionModeEnum Mode { get; set; }
    }
}
