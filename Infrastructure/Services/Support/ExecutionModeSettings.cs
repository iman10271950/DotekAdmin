using Application.Common.Interfaces.Auth;
using Domain.Enums.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Support
{
    public class ExecutionModeSettings : IExecutionMode
    {
        public ExecutionModeEnum Mode { get; set; } = ExecutionModeEnum.Debug;

    }
}
