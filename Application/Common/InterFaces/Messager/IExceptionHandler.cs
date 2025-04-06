using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.BaseEntites.Common;

namespace Application.Common.InterFaces.Messager
{
    public interface IExceptionHandler
    {
        BaseResult_VM<object> HandleException(Exception exception);
    }
}
